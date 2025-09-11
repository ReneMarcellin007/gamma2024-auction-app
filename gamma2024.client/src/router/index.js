import { createRouter, createWebHistory } from "vue-router";
import store from "@/store";
import Inscription from "@/components/views/Inscription.vue";
import Connexion from "@/components/views/Connexion.vue";
import Accueil from "@/components/views/Accueil.vue";
import Contact from "@/components/views/Contact.vue";
import APropos from "@/components/views/APropos.vue";
import TableauDeBordInventaire from "@/components/views/TableauDeBordInventaire.vue";
import EncanPresent from "@/components/views/EncanPresent.vue";
import EncansFuturs from "@/components/views/EncansFuturs.vue";
import EncansPasses from "@/components/views/EncansPasses.vue";
import TousLesEncans from "@/components/views/TousLesEncans.vue";
import Encan from "@/components/views/Encan.vue";
import DetailsLot from "@/components/views/DetailsLot.vue";
import AffichageVendeur from "@/components/views/AffichageVendeur.vue";
import VendeurCreation from "@/components/views/VendeurCreation.vue";
import VendeurModification from "@/components/views/VendeurModification.vue";
import AccesNonAutorise from "@/components/views/AccesNonAutorise.vue";
import ModificationProfilUtilisateur from "@/components/views/ModificationProfilUtilisateur.vue";
import GestionMembreParAdmin from "@/components/views/GestionMembreParAdmin.vue";
import DetailsMembreParAdmin from "@/components/views/DetailsMembreParAdmin.vue";
import AffichageLots from "@/components/views/AffichageLots.vue";
import CreationLot from "@/components/views/CreationLot.vue";
import ModificationLot from "@/components/views/ModificationLot.vue";
import ReinitialisationMotDePasse from "@/components/views/ReinitialisationMotDePasse.vue";
import TableauDeBordEncans from "@/components/views/TableauDeBordEncans.vue";
import TableauDeBordEncansAjout from "@/components/views/TableauDeBordEncansAjout.vue";
import ModificationEncan from "@/components/views/ModificationEncan.vue";
import EnregistrerCarte from "@/components/views/EnregistrerCarte.vue";
import TableauDeBordVentes from "@/components/views/TableauDeBordVentes.vue";
import ResultatRechercheLots from "@/components/views/ResultatRechercheLots.vue";
import ResultatRechercheEncans from "@/components/views/ResultatRechercheEncans.vue";
import GestionCartes from "@/components/views/GestionCartes.vue";
import HistoriqueAchatsParMembre from "@/components/views/HistoriqueAchatsParMembre.vue";
import HistoriqueMisesParMembre from "@/components/views/HistoriqueMisesParMembre.vue";
import ChoixLivraison from "@/components/views/ChoixLivraison.vue";
import ConditionsCompagnie from "@/components/views/ConditionsCompagnie.vue";
import ManuelUtilisateur from "@/components/views/ManuelUtilisateur.vue"

const routes = [
    {
        path: "/",
        name: "Accueil",
        component: Accueil,
        meta: { requiresAuth: false },
    },
    {
        path: "/accueil",
        name: "AccueilPage",
        component: Accueil,
        meta: { requiresAuth: false },
    },
    {
        path: "/contact",
        name: "Contact",
        component: Contact,
        meta: { requiresAuth: false },
    },
    {
        path: "/apropos",
        name: "APropos",
        component: APropos,
        meta: { requiresAuth: false },
    },
    {
        path: "/connexion",
        name: "Connexion",
        component: Connexion,
        meta: { requiresAuth: false },
    },
    {
        path: "/inscription",
        name: "Inscription",
        component: Inscription,
        meta: { requiresAuth: false },
    },
    {
        path: "/encanpresent",
        name: "EncanPresent",
        component: EncanPresent,
        meta: { requiresAuth: false },
    },
    {
        path: "/encansfuturs",
        name: "EncansFuturs",
        component: EncansFuturs,
        meta: { requiresAuth: false },
    },
    {
        path: "/encanspasses",
        name: "EncansPasses",
        component: EncansPasses,
        meta: { requiresAuth: false },
    },
    {
        path: "/touslesencans",
        name: "TousLesEncans",
        component: TousLesEncans,
        meta: { requiresAuth: false },
    },
    {
        path: "/detailslot/:idLot",
        name: "DetailsLot",
        component: DetailsLot,
        props: true,
        meta: { requiresAuth: false },
    },
    {
        path: "/inventaire",
        name: "TableauDeBordInventaire",
        component: TableauDeBordInventaire,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/modificationprofilutilisateur",
        name: "ModificationProfilUtilisateur",
        component: ModificationProfilUtilisateur,
        meta: { requiresAuth: true, requiredRole: "Client" },
        beforeEnter: async (to, from, next) => {
            if (store.state.isLoggedIn) {
                try {
                    await store.dispatch("fetchClientInfo");
                    next();
                } catch (error) {
                    console.error(
                        "Erreur lors du chargement des informations client:",
                        error
                    );
                    next("/error");
                }
            } else {
                next("/login");
            }
        },
    },
    {
        path: "/affichagevendeurs",
        name: "AffichageVendeurs",
        component: AffichageVendeur,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/vendeurcreation",
        name: "VendeurCreation",
        component: VendeurCreation,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/vendeurmodification/:id",
        name: "vendeurModification",
        component: VendeurModification,
        props: true, // Ceci permet de passer l'ID comme prop au composant
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/accesnonautorise",
        name: "AccesNonAutorise",
        component: AccesNonAutorise,
        meta: { requiresAuth: false },
    },
    {
        path: "/affichagelots",
        name: "AffichageLots",
        component: AffichageLots,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/lots/creation",
        name: "CreationLot",
        component: CreationLot,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/lots/modification/:id",
        name: "ModificationLot",
        component: ModificationLot,
        props: true,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/gestionMembre",
        name: "GestionMembre",
        component: GestionMembreParAdmin,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/detailsMembre/:id",
        name: "detailsMembre",
        component: DetailsMembreParAdmin,
        props: true,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/encan/:numeroEncan",
        name: "Encan",
        component: Encan,
        props: true,
        meta: { requiresAuth: false },
    },
    {
        path: "/tableaudebordencans",
        name: "TableauDeBordEncans",
        component: TableauDeBordEncans,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/tableaudebordencansajout",
        name: "TableauDeBordEncansAjout",
        component: TableauDeBordEncansAjout,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/ModificationEncan/:id",
        name: "ModificationEncan",
        component: ModificationEncan,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/ReinitialiserMotDePasse",
        name: "ReinitialiserMotDePasse",
        component: ReinitialisationMotDePasse,
    },
    {
        path: "/cartes/enregistrer",
        name: "EnregistrerCarte",
        component: EnregistrerCarte,
        props: true,
        meta: {
            requiresAuth: true,
            requiredRole: "Client",
        },
    },
    {
        path: "/cartes",
        name: "GestionCartes",
        component: GestionCartes,
        props: true,
        meta: {
            requiresAuth: true,
            requiredRole: "Client",
        },
    },
    {
        path: "/livraison/:idFacture",
        name: "ChoixLivraison",
        component: ChoixLivraison,
        props: true,
        meta: {
            requiresAuth: true,
            requiredRole: "Client",
        },
    },
    {
        path: "/tableaudebordventes",
        name: "TableauDeBordVentes",
        component: TableauDeBordVentes,
        meta: { requiresAuth: true, requiredRole: "Administrateur" },
    },
    {
        path: "/resultatrecherchelots",
        name: "ResultatRechercheLots",
        component: ResultatRechercheLots,
        meta: { requiresAuth: false },
    },
    {
        path: "/resultatrechercheencans",
        name: "ResultatRechercheEncans",
        component: ResultatRechercheEncans,
        meta: { requiresAuth: false },
    },
    {
        path: "/historiqueAchatsParMembre",
        name: "HistoriqueAchatsParMembre",
        component: HistoriqueAchatsParMembre,
        meta: { requiresAuth: true, requiredRole: "Client" },
    },
    {
        path: "/conditionsCompagnie",
        name: "ConditionsCompagnie",
        component: ConditionsCompagnie,
        meta: { requiresAuth: false },
    },
    {
        path: "/historiqueMisesParMembre",
        name: "HistoriqueMisesParMembre",
        component: HistoriqueMisesParMembre,
        meta: { requiresAuth: true, requiredRole: "Client" },
    },
    {
        path: "/ManuelUtilisateur",
        name: "ManuelUtilisateur",
        component: ManuelUtilisateur,
        meta: { requiresAuth: false },
    }
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
    scrollBehavior(to, from, savedPosition) {
        return { top: 0 };
    },
});

router.beforeEach(async (to, from, next) => {
  try {
    console.log("Navigation to:", to.path);

    // Vérifier si un token existe
    const token = localStorage.getItem("token");
    const isLoggedIn = store.state.isLoggedIn;

    if (token && !isLoggedIn) {
      console.log("Token found but not logged in, checking auth...");
      await store.dispatch("checkAuthStatus");
    }

    // Vérifier les autorisations
    if (to.meta.requiresAuth && !store.state.isLoggedIn) {
      next("/connexion");
      return;
    }

    if (
      to.meta.requiredRole &&
      !store.state.roles.includes(to.meta.requiredRole)
    ) {
      next({ name: "AccesNonAutorise" });
      return;
    }

    next();
  } catch (error) {
    console.error("Navigation error:", error);
    next("/connexion");
  }
});

export default router;
