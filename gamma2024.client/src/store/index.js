import { createStore } from "vuex";
import { initApi } from "@/services/api";
import { reactive } from "vue";
import {
  startSignalRConnection,
  stopSignalRConnection,
} from "@/services/signalR";
import { toast } from "vue3-toastify";
import { h } from "vue";
import ToastContent from "@/components/Toast/toastConfirm.vue";

const store = createStore({
  state: {
    api: null,
    token: null,
    roles: [],
    isLoggedIn: false,
    user: null,
    connection: null, // Connexion SignalR
    notificationConnection: null,
    lots: {},
    userBids: [],
    userOutbidLots: [],
    notifications: reactive([]),
    userBidHistory: {}, // Format: { lotId: { userId: montant } }
    encanCourant: null,
    lotsEncanCourant: {}, // Pour l'encan actif
    soireeCloture: null,
    nombreNotifNonLue: reactive(0),
  },
  mutations: {
    SET_NOTIFICATIONS(state, myNotifications) {
      if (Array.isArray(myNotifications)) {
        state.notifications = myNotifications;
      } else if (
        typeof myNotifications === "object" &&
        myNotifications !== null
      ) {
        state.notifications.push(myNotifications);
      }

      state.nombreNotifNonLue = state.notifications
        ? state.notifications.length
        : 0;
      console.log(state.nombreNotifNonLue);
    },

    MARK_AS_READ(state) {
      state.notifications = [];
      state.nombreNotifNonLue = 0;
    },

    setLoggedIn(state, value) {
      console.log("Setting logged in:", value);
      state.isLoggedIn = value;
      localStorage.setItem("isLoggedIn", value);
      if (!value) {
        state.userBids = [];
        if (state.lots) {
          Object.values(state.lots).forEach((lot) => {
            lot.userHasBid = false;
          });
        }
      }
    },
    setUser(state, user) {
      console.log("Setting user:", user?.id);
      if (user) {
        state.user = {
          id: user.id,
          username: user.username,
          name: user.name,
          firstName: user.firstName,
          email: user.email,
          phoneNumber: user.phoneNumber,
          pseudonym: user.pseudonym,
          photo: user.photo,
          roles: user.roles,
          cardOwnerName: user.cardOwnerName,
          cardNumber: user.cardNumber,
          cardExpiryDate: user.cardExpiryDate,
          civicNumber: user.civicNumber,
          street: user.street,
          apartment: user.apartment,
          city: user.city,
          province: user.province,
          country: user.country,
          postalCode: user.postalCode,
        };
        localStorage.setItem("user", JSON.stringify(state.user));
      } else {
        state.user = null;
        localStorage.removeItem("user");
      }
    },
    setRoles(state, roles) {
      console.log("Setting roles:", roles);
      state.roles = Array.isArray(roles) ? roles : [roles];
      localStorage.setItem("roles", JSON.stringify(state.roles));
    },
    setToken(state, token) {
      console.log("Setting token:", token ? "exists" : "none");
      state.token = token;
      if (token) {
        localStorage.setItem("token", token);
        if (state.api) {
          state.api.defaults.headers.common[
            "Authorization"
          ] = `Bearer ${token}`;
        }
      } else {
        localStorage.removeItem("token");
        if (state.api) {
          delete state.api.defaults.headers.common["Authorization"];
        }
      }
    },
    SET_API(state, api) {
      state.api = api;
      // Configurer le token s'il existe
      const token = localStorage.getItem("token");
      if (token) {
        state.api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
      }
    },
    SET_CLIENT_INFO(state, clientInfo) {
      state.clientInfo = clientInfo;
    },
    refreshUserData(state) {
      // Cette mutation ne fait rien, mais force la mise à jour des getters
      state.userDataVersion = (state.userDataVersion || 0) + 1;
    },
    updateLotMise(state, { idLot, montant, userId, userLastBid, nombreMises }) {
      const newLots = { ...state.lots };
      if (!newLots[idLot]) {
        newLots[idLot] = {
          id: idLot,
          mise: montant,
          idClientMise: userId,
          nombreMises: nombreMises,
        };
      } else {
        newLots[idLot] = {
          ...newLots[idLot],
          mise: montant,
          idClientMise: userId,
          nombreMises: nombreMises,
        };
      }
      state.lots = newLots;

      // Mettre à jour userBids
      if (userLastBid && userLastBid.userId === state.user?.id) {
        if (!state.userBids.includes(idLot)) {
          state.userBids.push(idLot);
        }
      }

      // Mettre à jour l'historique des mises
      if (userLastBid) {
        if (!state.userBidHistory[idLot]) {
          state.userBidHistory[idLot] = {};
        }
        state.userBidHistory[idLot][userLastBid.userId] = userLastBid.montant;
      }
    },

    SET_CONNECTION(state, connection) {
      if (state.connection) {
        // Nettoyer les écouteurs de l'ancienne connexion
        state.connection.off("ReceiveNewBid");
      }
      state.connection = connection;
    },

    SET_NOTIFICATION_CONNECTION(state, notificationConnection) {
      state.notificationConnection = notificationConnection;
    },
    addUserBid(state, lotId) {
      if (!state.userBids.includes(lotId)) {
        state.userBids.push(lotId);
      }
    },
    setUserBids(state, bids) {
      // Stocke les IDs des lots sur lesquels l'utilisateur a misé
      state.userBids = bids.map((bid) => bid.lotId);

      // Met à jour chaque lot avec son statut
      bids.forEach((bid) => {
        if (state.lots[bid.lotId]) {
          state.lots[bid.lotId] = {
            ...state.lots[bid.lotId],
            userHasBid: true, // L'utilisateur a misé sur ce lot
            isOutbid: !bid.isLastBidder, // L'utilisateur n'est plus le dernier enchérisseur
          };
        }
      });
    },
    setLots(state, lots) {
      // Convertir le tableau en objet indexé par id
      const lotsObj = {};
      lots.forEach((lot) => {
        // Conserver l'état userHasBid s'il existe déjà
        const existingLot = state.lots[lot.id];
        lotsObj[lot.id] = {
          ...lot,
          userHasBid: existingLot ? existingLot.userHasBid : false,
        };
      });
      state.lots = lotsObj;
    },
    refreshLots(state) {
      // Forcer la réactivité en créant une nouvelle référence
      state.lots = [...state.lots];
    },
    updateLotsWithUserBids(state, userBids) {
      if (!state.lots || !userBids) return;

      // Mettre à jour les lots avec les informations de mise
      Object.keys(state.lots).forEach((lotId) => {
        const userBid = userBids.find((bid) => bid.lotId === parseInt(lotId));
        const lot = state.lots[lotId];

        if (userBid) {
          // L'utilisateur a misé sur ce lot
          const isHighestBidder = userBid.montant === lot.mise;
          state.lots[lotId] = {
            ...lot,
            userHasBid: true,
            isOutbid: !isHighestBidder && userBid.montant < lot.mise,
          };
        }
      });
    },
    clearUserBids(state) {
      state.userBids = [];
      if (state.lots) {
        Object.values(state.lots).forEach((lot) => {
          lot.userHasBid = false;
        });
      }
    },

    setLotOutbid(state, lotId) {
      state.outbidLots.push(lotId);
    },
    setSignalRConnection(state, connection) {
      // Ajouter cette mutation manquante
      state.connection = connection;
    },
    updateUserBid(state, { lotId, userId, montant }) {
      if (!state.userBidHistory[lotId]) {
        state.userBidHistory[lotId] = {};
      }
      state.userBidHistory[lotId][userId] = montant;
    },
    UPDATE_USER_LAST_BID(state, { lotId, userId, montant }) {
      if (!state.userBidHistory) {
        state.userBidHistory = {}; // Initialiser l'objet s'il n'existe pas
      }
      if (!state.userBidHistory[lotId]) {
        state.userBidHistory[lotId] = {};
      }
      state.userBidHistory[lotId][userId] = montant;
    },
    SET_ENCAN_COURANT(state, encan) {
      state.encanCourant = encan;
    },
    SET_SOIREE_CLOTURE(state, soiree) {
      state.soireeCloture = soiree;
    },
    UPDATE_LOT_TEMPS(state, { lotId, nouveauTemps, ordreLotsActuel }) {
      const lot = state.lots[lotId];
      if (lot) {
        lot.dateFinDecompteLot = nouveauTemps;

        // Si on a reçu l'ordre des lots, réorganiser immédiatement
        if (ordreLotsActuel) {
          const lotsReorganises = {};
          ordreLotsActuel.forEach((id) => {
            if (state.lots[id]) {
              lotsReorganises[id] = state.lots[id];
            }
          });
          state.lots = lotsReorganises;
        }
      }
    },
    SET_LOT_VENDU(state, lotId) {
      const lot = state.lots[lotId];
      if (lot) {
        lot.estVendu = true;
        this.commit("REORGANISER_LOTS");
      }
    },
    REORGANISER_LOTS(state) {
      const lotsNonVendus = Object.values(state.lots)
        .filter((lot) => !lot.estVendu)
        .sort((a, b) => {
          // Si pas de date de fin, mettre à la fin
          if (!a.dateFinDecompteLot || !b.dateFinDecompteLot) return 0;

          const finA = new Date(a.dateFinDecompteLot);
          const finB = new Date(b.dateFinDecompteLot);

          // Si les dates de fin sont égales, comparer les dates de début
          if (finA.getTime() === finB.getTime()) {
            if (!a.dateDebutDecompteLot || !b.dateDebutDecompteLot) return 0;
            return (
              new Date(a.dateDebutDecompteLot) -
              new Date(b.dateDebutDecompteLot)
            );
          }

          return finA - finB;
        });

      const lotsReorganises = {};
      lotsNonVendus.forEach((lot) => {
        lotsReorganises[lot.id] = lot;
      });

      state.lots = lotsReorganises;
    },
    RESET_ENCAN_STATE(state) {
      state.encanCourant = null;
      state.soireeCloture = null;
    },
  },
  actions: {
    async login({ commit, state, dispatch }, userData) {
      try {
        if (!state.api) {
          throw new Error("API non initialisée");
        }
        const reponse = await state.api.post("/home/login", userData);
        if (reponse.data && reponse.data.message === "Connexion réussie") {
          commit("setLoggedIn", true);

          // Sauvegarder le token
          const token = reponse.data.token;
          localStorage.setItem("token", token);

          // Sauvegarder l'utilisateur
          const user = {
            id: reponse.data.userId,
            username: reponse.data.username,
            name: reponse.data.name,
            firstName: reponse.data.firstName,
            roles: reponse.data.roles,
            photo: reponse.data.photo,
          };
          localStorage.setItem("userData", JSON.stringify(user));
          localStorage.setItem("userRoles", JSON.stringify(reponse.data.roles));

          // Mettre à jour le store
          commit("setToken", token);
          commit("setUser", user);
          commit("setRoles", reponse.data.roles);
          commit("setLoggedIn", true);

          // Initialiser les connexions
          await dispatch("initializeSignalR");
          try {
            await dispatch("obtenirNotification", user.id);
            await dispatch("initNotificationConnection");
          } catch (err) {
            console.warn("Erreur notifications:", err);
          }

          return { success: true, roles: reponse.data.roles };
        }
        return { success: false, error: "Réponse inattendue" };
      } catch (error) {
        throw error;
      }
    },

    async fetchClientInfo({ state, commit }) {
      try {
        const reponse = await state.api.get(
          "/Utilisateurs/ObtentionInfoClient"
        );
        commit("SET_CLIENT_INFO", reponse.data);
        // Assurez-vous également de mettre à jour l'utilisateur
        commit("setUser", reponse.data);
        return reponse.data;
      } catch (error) {
        console.error(
          "Erreur lors de la récupération des informations du client:",
          error
        );
        throw error.response?.data || error.message;
      }
    },

    async updateClientInfo({ state, commit }, userData) {
      try {
        const reponse = await state.api.put(
          "/utilisateurs/miseajourinfoclient",
          userData
        );
        const updatedUser = { ...state.user, ...reponse.data };
        commit("setUser", updatedUser);

        // Forcer la mise à jour des getters
        commit("refreshUserData");

        return reponse;
      } catch (error) {
        console.error(
          "Erreur lors de la mise à jour des informations du client:",
          error.reponse || error
        );
        throw error.response?.data || error.message;
      }
    },

    async updateAvatar({ commit, state }, formData) {
      try {
        const reponse = await state.api.put("/utilisateurs/avatar", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });

        // Construire l'URL complète de l'avatar
        const avatarPath = reponse.data.avatarUrl.startsWith("/")
          ? reponse.data.avatarUrl
          : `/Avatars/${reponse.data.avatarUrl}`;
        const fullAvatarUrl = `${state.api.defaults.baseURL.replace(
          "/api",
          ""
        )}${avatarPath}`;

        // Mettre à jour l'utilisateur avec la nouvelle URL de l'avatar
        const updatedUser = { ...state.user, photo: avatarPath };
        commit("setUser", updatedUser);

        return {
          ...reponse,
          data: { ...reponse.data, avatarUrl: fullAvatarUrl },
        };
      } catch (error) {
        console.error(
          "Erreur détaillée lors de la mise à jour de l'avatar:",
          error.reponse || error
        );
        throw error.response?.data || error.message;
      }
    },

    async verifierPseudonyme({ commit, state }, pseudo) {
      try {
        const reponse = await state.api.get(
          `/utilisateurs/verifier-pseudonyme?pseudo=${encodeURIComponent(
            pseudo
          )}`
        );
        return reponse.data.disponible;
      } catch (error) {
        console.error("Erreur lors de la vérification du pseudonyme:", error);
        throw error.response?.data || error.message;
      }
    },

    async verifierEmail({ commit, state }, email) {
      try {
        const reponse = await state.api.get(
          `/utilisateurs/verifier-email?email=${encodeURIComponent(email)}`
        );
        return reponse.data.disponible;
      } catch (error) {
        console.error("Erreur lors de la vérification de l'email:", error);
        throw error.response?.data || error.message;
      }
    },

    async obtenirTousVendeurs({ commit, state }) {
      try {
        const reponse = await state.api.get("/vendeurs/tous");
        return reponse.data;
      } catch (error) {
        console.error(
          "Erreur lors de la récupération de tous les vendeurs:",
          error
        );
        throw error.response?.data || error.message;
      }
    },
    async creerVendeur({ commit, state }, vendeurData) {
      try {
        const reponse = await state.api.post("/vendeurs/creer", vendeurData);
        if (reponse.data.success) {
          return { success: true, message: reponse.data.message };
        } else {
          return { success: false, error: reponse.data.message };
        }
      } catch (error) {
        console.error("Erreur détaillée lors de la création du vendeur:", error);
        return {
          success: false,
          error: error.response?.data?.message || 
                error.message || 
                "Erreur lors de la création du vendeur"
        };
      }
    },
    async modifierVendeur({ commit, state }, vendeurData) {
      try {
        const reponse = await state.api.put(
          `/vendeurs/modifier/${vendeurData.id}`,
          vendeurData
        );
        if (reponse.data.success) {
          return { success: true, message: reponse.data.message };
        } else {
          return { success: false, error: reponse.data.message };
        }
      } catch (error) {
        console.error("Erreur lors de la modification du vendeur:", error);
        return {
          success: false,
          error: error.response?.data?.message || 
                "Erreur lors de la modification du vendeur"
        };
      }
    },
    async obtenirVendeur({ commit, state }, id) {
      try {
        const reponse = await state.api.get(`/vendeurs/${id}`);
        return reponse.data;
      } catch (error) {
        console.error("Erreur lors de la récupération du vendeur:", error);
        throw error.response?.data || error.message;
      }
    },
    async creerLot({ state }, formData) {
      try {
        const reponse = await state.api.post("/lots/creer", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });
        return reponse.data;
      } catch (error) {
        console.error("Erreur lors de la création du lot:", error);
        throw error.response?.data || error.message;
      }
    },

    async modifierLot({ state }, { id, lotData }) {
      try {
        const reponse = await state.api.put(`/lots/modifier/${id}`, lotData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });
        return reponse.data;
      } catch (error) {
        console.error("Erreur lors de la modification du lot:", error);
        throw error.response?.data || error.message;
      }
    },

    async obtenirTousLots({ state }) {
      try {
        const reponse = await state.api.get("/lots/tous");
        return reponse.data || [];
      } catch (error) {
        console.error(
          "Erreur lors de la récupération de tous les lots:",
          error
        );
        throw error.response?.data || error.message;
      }
    },

    async obtenirLot({ state }, id) {
      try {
        const reponse = await state.api.get(`/lots/${id}`);
        return reponse.data;
      } catch (error) {
        console.error("Erreur lors de la récupération du lot:", error);
        throw error.response?.data || error.message;
      }
    },

    async supprimerLot({ state }, id) {
      try {
        const reponse = await state.api.delete(`/lots/supprimer/${id}`);
        return reponse.data;
      } catch (error) {
        console.error("Erreur lors de la suppression du lot:", error);
        throw error.response?.data || error.message;
      }
    },
    async obtenirCategories({ state }) {
      const reponse = await state.api.get("/lots/categories");
      return reponse.data;
    },
    async obtenirVendeurs({ state }) {
      const reponse = await state.api.get("/lots/vendeurs");
      return reponse.data;
    },
    async obtenirMediums({ state }) {
      const reponse = await state.api.get("/lots/mediums");
      return reponse.data;
    },
    async obtenirEncans({ state }) {
      const reponse = await state.api.get("/lots/encans");
      return reponse.data;
    },
    async ObtenirTousLesMembres({ commit, state }) {
      try {
        const reponse = await state.api.get(
          `/administrateur/ObtenirTousLesUsers`
        );
        return reponse.data;
      } catch (error) {
        console.error(
          "Erreur détaillée:",
          error.reponse?.data || error.message
        );
        throw error.response?.data || error.message;
      }
    },

    async obtenirUnMembre({ commit, state }, membreId) {
      try {
        const reponse = await state.api.get(
          `/administrateur/obtenirTousLesMembres/${membreId}`
        );
        return reponse.data;
      } catch (error) {
        console.error("Erreur lors de la récupération du membre:", error);
        throw error.response?.data || error.message;
      }
    },

    async bloquerUnMembre({ commit, state }, membreId) {
      try {
        const reponse = await state.api.get(
          `/administrateur/bloquerMembre/${membreId}`
        );
        return reponse.data;
      } catch (error) {
        console.error("Erreur lors du blocage du membre:", error);
        throw error.response?.data || error.message;
      }
    },

    async debloquerUnMembre({ commit, state }, membreId) {
      try {
        const reponse = await state.api.get(
          `/administrateur/debloquerMembre/${membreId}`
        );
        return reponse.data;
      } catch (error) {
        console.error("Erreur lors du déblocage du membre:", error);
        throw error.response?.data || error.message;
      }
    },
    async checkAuthStatus({ commit, state, dispatch }) {
      try {
        const token = localStorage.getItem("token");
        const userData = JSON.parse(localStorage.getItem("userData"));
        const userRoles = JSON.parse(localStorage.getItem("userRoles"));

        if (!token || !userData) {
          throw new Error("Données d'authentification manquantes");
        }

        // Configurer l'API
        state.api.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        // Vérifier l'authentification
        const response = await state.api.get("/home/check-auth");

        if (response.data.isAuthenticated) {
          commit("setToken", token);
          commit("setUser", userData);
          commit("setRoles", userRoles);
          commit("setLoggedIn", true);

          // Réinitialiser les connexions
          await dispatch("initializeSignalR");
          return true;
        }
        throw new Error("Non authentifié");
      } catch (error) {
        // Nettoyer le stockage local
        localStorage.removeItem("token");
        localStorage.removeItem("userData");
        localStorage.removeItem("userRoles");

        // Réinitialiser le store
        commit("setToken", null);
        commit("setUser", null);
        commit("setRoles", []);
        commit("setLoggedIn", false);

        return false;
      }
    },
    async logout({ commit, state }) {
      try {
        await stopSignalRConnection();
        commit("SET_CONNECTION", null);
        // Appel à l'API pour invalider le token côté serveur
        const reponse = await state.api.post("/home/logout");
      } catch (error) {
        console.error("Erreur lors de la déconnexion:", error);
      } finally {
        // Nettoyage des données côté client
        commit("setLoggedIn", false);
        commit("setUser", null);
        commit("setRoles", []);
        commit("setToken", null);
        commit("SET_CONNECTION", null); // Nettoyer la référence SignalR

        // Nettoyage du stockage local
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        localStorage.removeItem("roles");
        localStorage.removeItem("isLoggedIn");

        // Réinitialiser les données liées aux mises
        commit("updateLotMise", {
          idLot: null,
          montant: null,
          userId: null,
        });
        state.userBids = [];
        state.lots = {};
      }
    },

    async initializeStore({ commit, dispatch }) {
      try {
        const token = localStorage.getItem("token");
        const savedIsLoggedIn = localStorage.getItem("isLoggedIn") === "true";
        const savedUser = JSON.parse(localStorage.getItem("user"));
        const savedRoles = JSON.parse(localStorage.getItem("roles")) || [];

        const api = initApi(() => token);
        if (token) {
          api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
        }
        commit("SET_API", api);

        commit("setToken", token);
        commit("setLoggedIn", savedIsLoggedIn);
        commit("setUser", savedUser);
        commit("setRoles", savedRoles);

        if (savedIsLoggedIn) {
          await dispatch("fetchUserBids");
          await dispatch("initializeSignalR");
          await dispatch("initNotificationConnection");
          await dispatch("verifierEtatEncan");
        }
      } catch (error) {
        console.error("Erreur lors de l'initialisation du store:", error);
        // Réinitialiser l'état en cas d'erreur
        commit("setLoggedIn", false);
        commit("setUser", null);
        commit("setRoles", []);
        commit("setToken", null);
      }
    },

    async chercherTousEncansVisibles({ commit, state }) {
      try {
        const reponse = await state.api.get(
          "/encans/cherchertousencansvisibles"
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },
    async chercherEncanParNumero({ commit, state }, numeroEncan) {
      try {
        const reponse = await state.api.get(
          "/encans/chercherencanparnumero/" + numeroEncan
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },
    async chercherEncanEnCours({ commit, state }) {
      try {
        const reponse = await state.api.get("/encans/chercherencanencours");
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async chercherNumeroEncanEnCours({ commit, state }) {
      try {
        const reponse = await state.api.get(
          "/encans/ChercherNumeroEncanEnCours"
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async fetchEncanInfo({ commit, state }) {
      try {
        const reponse = await state.api.get("/encans/cherchertousencans");

        return reponse.data;
      } catch (error) {
        console.error("Erreur détaillée:", error.reponse || error);
        throw error.response?.data || error.message;
      }
    },
    async supprimerUnEncan({ commit, state }, numeroEncan) {
      try {
        const reponse = await state.api.delete(
          `/encans/supprimerEncan/${numeroEncan}`
        );
        return reponse.data;
      } catch (error) {
        console.error(
          "Erreur détaillée lors de la suppression de l'encan",
          error.reponse || error
        );
        throw error.response?.data || error.message;
      }
    },
    async obtenirUnEncanParId({ commit, state }, idEncan) {
      try {
        const reponse = await state.api.get(
          `/encans/obtenirUnEncan/${idEncan}`
        );
        return reponse.data;
      } catch (error) {
        console.error(
          "Erreur détaillée lors de la suppression de l'encan",
          error.reponse || error
        );
        throw error.response?.data || error.message;
      }
    },

    async modifierEncan({ commit, state }, encanData) {
      try {
        const reponse = await state.api.put(
          `/encans/modifierEncan/${encanData.id}`,
          encanData
        );
        if (reponse.data.success) {
          return { success: true, message: reponse.data.message };
        } else {
          return { success: false, error: reponse.data.message };
        }
      } catch (error) {
        return {
          success: false,
          error:
            error.reponse?.data?.message ||
            "Erreur lors de la modification de l'encan",
        };
      }
    },

    async creerEncan({ commit, state }, encanData) {
      try {
        const reponse = await state.api.post("/encans/creerEncan", encanData);
        if (reponse.data.sucess) {
          return { success: true, message: reponse.data.message };
        } else {
          return { success: false, error: reponse.data.message };
        }
      } catch (error) {
        return {
          success: false,
          error: error.reponse.data.message,
          details: error.reponse?.data,
        };
      }
    },

    async mettreAJourEncanPublie({ commit, state }, encanData) {
      try {
        const reponse = await state.api.put(
          "/encans/mettreAJourEncanPublie",
          encanData
        );
        if (reponse.data.sucess) {
          return { success: true, message: reponse.data.message };
        } else {
          return { success: false, error: reponse.data.message };
        }
      } catch (error) {
        return {
          success: false,
          error: error.reponse.data.message,
          details: error.reponse?.data,
        };
      }
    },

    async chercherTousLotsRecherche({ state }) {
      try {
        const reponse = await state.api.get(`/lots/cherchertouslotsrecherche`);
        return reponse;
      } catch (error) {
        console.error("Erreur détaillée:", {
          message: error.message,
          status: error.reponse?.status,
          data: error.reponse?.data,
          config: error.config,
        });
        throw error.response?.data || error.message;
      }
    },
    async chercherTousLotsParEncan({ state }, idEncan) {
      try {
        const reponse = await state.api.get(
          `/lots/cherchertouslotsparencan/${idEncan}`
        );

        return reponse;
      } catch (error) {
        console.error("Erreur détaillée:", {
          message: error.message,
          status: error.reponse?.status,
          data: error.reponse?.data,
          config: error.config,
        });
        throw error.response?.data || error.message;
      }
    },
    async chercherDetailsLotParId({ commit, state }, idLot) {
      try {
        const reponse = await state.api.get(
          "/lots/chercherDetailsLotParId/" + idLot
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async chercherEncansFuturs({ commit, state }) {
      try {
        const reponse = await state.api.get("/encans/chercherencansfuturs");
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async chercherEncansPasses({ commit, state }) {
      try {
        const reponse = await state.api.get("/encans/chercherencanspasses");
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },
    forceUpdate({ commit }) {
      commit("refreshUserData");
    },

    async placerMise({ state, commit, dispatch }, miseData) {
      try {
        const reponse = await state.api.post("/lots/placerMise", {
          LotId: miseData.lotId,
          Montant: parseFloat(miseData.montant),
          UserId: state.user?.id,
          MontantMaximal: miseData.montantMaximal,
        });

        return reponse.data;
      } catch (error) {
        console.error("Store - Erreur lors de la mise:", error);
        return {
          success: false,
          message: error.reponse?.data?.message || "Erreur lors de la mise",
        };
      }
    },

    async fetchUserBids({ state, commit }) {
      if (!state.user?.id || !state.token) {
        console.log("Pas d'utilisateur ou de token, abandon de fetchUserBids");
        return;
      }

      console.log("Tentative fetchUserBids:", {
        userId: state.user.id,
        hasToken: !!state.token,
      });

      try {
        const reponse = await state.api.get(`/lots/userBids/${state.user.id}`, {
          headers: {
            Authorization: `Bearer ${state.token}`,
          },
        });
        commit("setUserBids", reponse.data);
      } catch (error) {
        console.error("Erreur lors du chargement des mises:", error);
        // Si erreur d'authentification, nettoyer les données
        if (error.reponse?.status === 401) {
          commit("setUserBids", []);
        }
      }
    },

    async initializeSignalR({ commit, state }) {
      if (state.connection) {
        return;
      }

      if (!state.isLoggedIn) return;

      try {
        const baseUrl = state.api.defaults.baseURL.replace("/api", "");
        const connection = await startSignalRConnection(baseUrl, state.token);

        if (connection) {
          // Gestion des mises normales
          connection.on("ReceiveNewBid", (data) => {
            commit("updateLotMise", {
              idLot: data.idLot,
              montant: data.montant,
              userId: data.userId,
            });

            if (data.userLastBid?.userId === state.user?.id) {
              commit("UPDATE_USER_LAST_BID", {
                lotId: data.idLot,
                userId: data.userLastBid.userId,
                montant: data.userLastBid.montant,
              });
            }
          });

          // Garder aussi l'événement LotVendu séparé pour compatibilité
          connection.on("LotVendu", (lotId) => {
            commit("SET_LOT_VENDU", lotId);
          });

          commit("SET_CONNECTION", connection);
        }
      } catch (error) {
        console.error("Erreur lors de l'initialisation de SignalR:", error);
      }
    },

    async reinitialisePassword({ commit, state }, resetPasswordData) {
      try {
        const reponse = await state.api.post(
          "/utilisateurs/reinitialiserMotDePasse",
          resetPasswordData
        );
        return {
          success: true,
          message: reponse.data,
        };
      } catch (error) {
        return {
          success: false,
          message:
            error.reponse?.data ||
            "Erreur lors de la modification du mot de passe.",
        };
      }
    },
    async creerCompteUtilisateur({ commit, state }, formData) {
      try {
        const reponse = await state.api.post("/utilisateurs/creer", formData);

        if (reponse.data.success) {
          return {
            success: true,
            message: reponse.data.message,
          };
        } else {
          return {
            success: false,
            message: reponse.data.message,
          };
        }
      } catch (error) {
        console.error("Erreur lors de la création du compte:", error);
        return {
          success: false,
          message:
            error.reponse?.data?.message ||
            "Une erreur est survenue lors de la création du compte",
        };
      }
    },
    async fetchFactureInfoMembre({ commit, state }) {
      try {
        const reponse = await state.api.get("/factures/chercherFacturesMembre");
        return reponse.data;
      } catch (error) {
        console.error("Erreur détaillée:", error.reponse || error);
        throw error;
      }
    },

    async obtenirArtistes({ state }) {
      const reponse = await state.api.get("/lots/artistes");
      return reponse.data;
    },

    async creerSetupIntent({ state }) {
      try {
        const reponse = await state.api.post("/paiement/creerSetupIntent");
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async chercherCartesUser({ state }) {
      try {
        const reponse = await state.api.get("/paiement/chercherCartes");
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },
    async fetchFactureInfo({ commit, state }) {
      try {
        const reponse = await state.api.get("/factures/chercherFactures");

        return reponse.data;
      } catch (error) {
        console.error("Erreur détaillée:", error.reponse || error);
        throw error;
      }
    },

    async creerPaymentIntent({ state }, idFacture) {
      try {
        const reponse = await state.api.post(
          "/paiement/creerPaymentIntent/" + idFacture
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },
    async chercherPrevisualisationLivraison({ state }, idFacture) {
      try {
        const reponse = await state.api.get(
          "/facturesLivraison/GenererFactureLivraison/" + idFacture
        );
        return reponse;
      } catch (error) {
        console.error("Erreur détaillée:", error.reponse || error);
        throw error;
      }
    },

    async chercherAdressesClient({ state }) {
      try {
        const reponse = await state.api.get(
          "/utilisateurs/chercheradressesclient"
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async enregistrerChoixLivraison({ state }, choixLivraison) {
      try {
        const reponse = await state.api.post(
          "/facturesLivraison/enregistrerChoixLivraison",
          choixLivraison
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async chercherDetailsFactureLivraison({ state }, idFactureLivraison) {
      try {
        const reponse = await state.api.get(
          "/facturesLivraison/chercherDetailsFactureLivraison/" +
            idFactureLivraison
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async chercherDetailsFacture({ state }, idFacture) {
      try {
        const reponse = await state.api.get(
          "/factures/chercherDetailsFacture/" + idFacture
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async supprimerCarte({ state }, pmId) {
      try {
        const reponse = await state.api.post(
          "/paiement/supprimerCarte/" + pmId
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },

    async getUserBidForLot({ state, commit }, lotId) {
      // Ne faire l'appel que si l'utilisateur est connecté
      if (!state.isLoggedIn) return 0;

      try {
        const reponse = await state.api.get(`/lots/userLastBid/${lotId}`);
        if (reponse.data > 0) {
          commit("UPDATE_USER_LAST_BID", {
            lotId,
            userId: state.user?.id,
            montant: reponse.data,
          });
        }
        return reponse.data;
      } catch (error) {
        console.error(
          "Erreur lors de la récupération de la dernière mise:",
          error
        );
        return 0;
      }
    },
    async chargerClientsFinEncan({ state }, numeroEncan) {
      try {
        const reponse = await state.api.post(
          "/factures/CreerFacturesParEncan/" + 232
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },
    async chercherFacturesParEncan({ state }, numeroEncan) {
      try {
        const reponse = await state.api.get(
          "/factures/chercherFacturesParEncan/" + 232
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },
    async chercherFacturesChoixAFaire({ state }) {
      try {
        const reponse = await state.api.get(
          "/factures/chercherFacturesChoixAFaire"
        );
        return reponse;
      } catch (error) {
        return "Erreur, veuillez réessayer";
      }
    },
    async verifierEtatEncan({ state, commit }) {
      const reponse = await state.api.get("/encans/etat-courant");
      const { type, encan } = reponse.data;

      commit("SET_ENCAN_COURANT", encan);

      if (encan?.id) {
        // Charger les lots séparément pour avoir toutes les informations
        const lotsreponse = await state.api.get(
          `/lots/cherchertouslotsparencan/${encan.id}`
        );
        if (lotsreponse.data) {
          commit("setLots", lotsreponse.data);
        }
      }

      return type; // 'courant' ou 'soireeCloture' ou null
    },
    async surveillerTransitionEncan({ dispatch, commit }) {
      const verifierEtat = async () => {
        const ancienType = this.state.typeEncanCourant;
        const nouveauType = await dispatch("verifierEtatEncan");

        if (ancienType !== nouveauType) {
          if (ancienType === "courant" && nouveauType === "soireeCloture") {
            await dispatch("initialiserSoireeCloture");
          } else if (nouveauType === "aucun") {
            commit("RESET_ENCAN_STATE");
          }
        }
      };
      setInterval(verifierEtat, 5000);
    },

    async initialiserSoireeCloture({ commit, dispatch }) {
      try {
        // Synchroniser les temps avec le serveur
        if (this.state.connection) {
          await this.state.connection.invoke("DemanderSynchronisation");
        }

        // Démarrer la surveillance des transitions
        await dispatch("surveillerTransitionEncan");
      } catch (error) {
        console.error(
          "Erreur lors de l'initialisation de la soirée de clôture:",
          error
        );
      }
    },

    async synchroniserTempsLots({ commit }, tempsLots) {
      Object.entries(tempsLots).forEach(([lotId, temps]) => {
        commit("UPDATE_LOT_TEMPS", {
          lotId: parseInt(lotId),
          nouveauTemps: temps,
        });
      });
      commit("REORGANISER_LOTS");
    },

    async surveillerFinSoireeCloture({ commit, state, router }) {
      if (state.connection) {
        state.connection.on("ReceiveNewBid", (data) => {
          if (data.type === "encanTermine") {
            toast.success(
              h(ToastContent, {
                title: "Soirée terminée",
                description: `La soirée de clôture de l'encan ${data.numeroEncan} est terminée. Les factures ont été générées.`,
              })
            );
          }
        });
        commit("RESET_ENCAN_STATE");
        router.push({ name: "Accueil" });
      }
    },

    async obtenirNotification({ state, commit }, userId) {
      try {
        // Vérifier si l'utilisateur est connecté et a un token valide
        if (!state.token || !userId) {
          console.log(
            "Pas de token ou userId, abandon de la requête notifications"
          );
          return;
        }

        const reponse = await state.api.get(
          `Notification/obtenirNotificationNonLu/${userId}`,
          {
            headers: {
              Authorization: `Bearer ${state.token}`,
            },
          }
        );

        if (reponse.data) {
          commit("SET_NOTIFICATIONS", reponse.data);
        }
      } catch (error) {
        console.error("Erreur notifications:", {
          message: error.message,
          status: error.response?.status,
          data: error.response?.data,
        });
        // Ne pas bloquer le processus de connexion si les notifications échouent
        commit("SET_NOTIFICATIONS", []);
      }
    },

    async marquerToutesNotifLues({ state, commit }) {
      try {
        const reponse = await state.api.post("notification/marquerLues");
        if (reponse.data) {
          commit("MARK_AS_READ");
        }
      } catch (error) {
        console.error("Error fetching notifications:", error);
      }
    },

    async initNotificationConnection({ commit, state }) {
      const baseUrl = state.api.defaults.baseURL.replace("/api", "");
      const cleanBaseUrl = baseUrl.endsWith("/")
        ? baseUrl.slice(0, -1)
        : baseUrl;

      const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${cleanBaseUrl}/api/hub/NotificationHub`, {
          accessTokenFactory: () => state.token,
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets,
          // withCredentials: true,
          headers: { "X-Requested-With": "XMLHttpRequest" },
        })
        .configureLogging(signalR.LogLevel.Debug)
        .withAutomaticReconnect()
        .build();

      // Écoute les événements SignalR
      connection.on("ReceiveNotification", (notification) => {
        commit("SET_NOTIFICATIONS", notification); // Ajouter au store
      });

      connection.onclose(() => {
        console.warn("SignalR connection closed.");
      });

      try {
        await connection.start();
        commit("SET_NOTIFICATION_CONNECTION", connection);
      } catch (err) {
        console.error("Error starting SignalR connection:", err);
      }
    },

    async sendNotification({ state }, { userId, message }) {
      if (state.notificationConnection) {
        try {
          await state.notificationConnection.invoke(
            "SendNotification",
            userId,
            message
          );
        } catch (err) {
          console.error("Error sending notification:", err);
        }
      }
    },

    async getUserBidsGroupedByEncan({ state }) {
      try {
        const response = await state.api.get("/lots/userBidsGroupedByEncan");
        console.log("Réponse de la requête:", response.data);
        return response.data;
      } catch (error) {
        console.error(
          "Erreur lors de la récupération des mises par encan:",
          error
        );
        throw error;
      }
    },
  },
  getters: {
    isAdmin: (state) => {
      const result =
        Array.isArray(state.roles) && state.roles.includes("Administrateur");
      return result;
    },
    isClient: (state) =>
      Array.isArray(state.roles) && state.roles.includes("Client"),
    currentUser: (state) => state.user,
    username: (state) =>
      state.user ? state.user.pseudonym || state.user.username : "USERNAME",
    avatarUrl: (state) => {
      if (state.user && state.user.photo) {
        if (state.user.photo.startsWith("http")) {
          return state.user.photo;
        } else {
          const baseUrl = state.api.defaults.avatarURL;
          const fullUrl = baseUrl + state.user.photo;
          return fullUrl;
        }
      }
      return "/gamma2024.client/public/icons/Avatar.png";
    },
    hasUserBidOnLot: (state) => (lotId) => {
      return state.userBids.includes(lotId);
    },
    getLot: (state) => (id) => {
      return state.lots[id] || null;
    },
    getAllLots: (state) => {
      return Object.values(state.lots);
    },
    getUniqueOffersCount: (state) => (lotId) => {
      const lot = state.lots[lotId];
      return lot?.nombreMises || 0;
    },
    // allNotifications: (state) => state.notifications,
    // unreadNotifications: (state) => state.nombreNotifNonLue,
  },
});

// Initialiser le store immédiatement
store.dispatch("initializeStore");

export default store;
