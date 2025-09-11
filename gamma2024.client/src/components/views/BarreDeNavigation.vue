<template>
    <header class="sticky-top">
        <div>
            <nav class="navbar navbar-expand-lg bleuMarinSecondaireFond py-0"
                 data-bs-theme="dark">
                <div class="container-fluid justify-content-between">
                    <router-link :to="{ name: 'Accueil' }" class="text-decoration-none">
                        <a class="navbar-brand d-flex align-items-center fs-6">
                            <img src="/images/Logo.png"
                                 alt="Les Encans de Nantes"
                                 height="40" />
                            Les Encans de Nantes <br />au Québec
                        </a>
                    </router-link>

                    <button class="navbar-toggler"
                            data-bs-theme="dark"
                            type="button"
                            data-bs-toggle="collapse"
                            data-bs-target="#navbarSupportedContent"
                            aria-controls="navbarSupportedContent"
                            aria-expanded="false"
                            aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>

                    <div class="collapse navbar-collapse justify-content-between"
                         id="navbarSupportedContent">
                        <ul class="navbar-nav text-center">
                            <li class="nav-item align-self-center d-flex align-items-center nav-link">
                                <router-link :to="{ name: 'Accueil' }"
                                             class="text-decoration-none">
                                    Accueil
                                </router-link>
                            </li>

                            <li class="nav-item nav-link">
                                <router-link :to="{ name: 'EncanPresent' }"
                                             class="text-decoration-none">
                                    Encan courant
                                </router-link>
                            </li>

                            <li class="nav-item nav-link">
                                <router-link :to="{ name: 'EncansPasses' }"
                                             class="text-decoration-none">
                                    Encans passés
                                </router-link>
                            </li>

                            <li class="nav-item nav-link">
                                <router-link :to="{ name: 'EncansFuturs' }"
                                             class="text-decoration-none">
                                    Encans futurs
                                </router-link>
                            </li>

                            <li class="nav-item nav-link" v-if="estConnecte && estClient">
                                <router-link :to="{ name: 'HistoriqueAchatsParMembre' }"
                                             class="text-decoration-none">
                                    Historique des achats
                                </router-link>
                            </li>
                            <li class="nav-item nav-link" v-if="estConnecte && estClient">
                                <router-link :to="{ name: 'HistoriqueMisesParMembre' }"
                                             class="text-decoration-none">
                                    Historique des mises
                                </router-link>
                            </li>
                        </ul>

                        <div class="d-flex justify-content-center gap-3 mb-2 mb-lg-0 flex-row align-items-center justify-content-center">
                            <router-link :to="{ name: 'Inscription' }" v-if="!estConnecte">
                                <button class="btn btn-outline btnBarreNavBleuMoyenFond text-white py-0 butttonNavBar btnSurvolerBleuMoyenFond"
                                        type="button">
                                    Inscription
                                </button>
                            </router-link>
                            <router-link :to="{ name: 'Connexion' }" v-if="!estConnecte">
                                <button class="btn btn-outline btnBarreNavBleuMoyenFond text-white py-0 butttonNavBar btnSurvolerBleuMoyenFond"
                                        type="button">
                                    Connexion
                                </button>
                            </router-link>

                            <div class="dropdown text-white align-self-stretch justify-content-center align-items-center d-flex"
                                 v-if="estAdmin">
                                <a class="nav-link dropdown-toggle"
                                   role="button"
                                   data-bs-toggle="dropdown"
                                   aria-expanded="false">
                                    Tableau de bord
                                </a>
                                <ul class="dropdown-menu bleuMarinFond text-center">
                                    <li>
                                        <router-link :to="{ name: 'TableauDeBordInventaire' }"
                                                     class="text-decoration-none">
                                            <a class="dropdown-item contenuListeDropdown">Inventaire</a>
                                        </router-link>
                                    </li>
                                    <li>
                                        <router-link :to="{ name: 'TableauDeBordEncans' }"
                                                     class="text-decoration-none">
                                            <a class="dropdown-item contenuListeDropdown">Encans</a>
                                        </router-link>
                                    </li>
                                    <li>
                                        <router-link :to="{ name: 'AffichageVendeurs' }"
                                                     class="text-decoration-none">
                                            <a class="dropdown-item contenuListeDropdown">Vendeurs</a>
                                        </router-link>
                                    </li>
                                    <li>
                                        <router-link :to="{ name: 'TableauDeBordVentes' }"
                                                     class="text-decoration-none">
                                            <a class="dropdown-item contenuListeDropdown">Ventes</a>
                                        </router-link>
                                    </li>
                                    <li>
                                        <router-link :to="{ name: 'GestionMembre' }"
                                                     class="text-decoration-none">
                                            <a class="dropdown-item contenuListeDropdown">Profils de membre</a>
                                        </router-link>
                                    </li>
                                </ul>
                            </div>

                            <div class="dropdown text-white align-self-stretch justify-content-center align-items-center d-flex"
                                 v-if="estConnecte && !estAdmin">
                                <button class="btn btn-link position-relative"
                                        id="dropdownNotification"
                                        data-bs-toggle="dropdown"
                                        aria-expanded="false">
                                    <img src="/icons/Cloche.png" alt="Icon cloche" height="25" />
                                    <span v-if="nombreNotificationsNonLues > 0"
                                          class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                                        {{ nombreNotificationsNonLues }}
                                        <span class="visually-hidden">notifications non lues</span>
                                    </span>
                                </button>
                                <ul class="dropdown-menu dropdown-menu-end"
                                    aria-labelledby="dropdownNotification">
                                    <li v-if="notifications.length === 0"
                                        class="dropdown-item text-muted">
                                        Aucune notification
                                    </li>
                                    <li v-else
                                        v-for="(notification, index) in notifications"
                                        :key="index"
                                        class="dropdown-item">
                                        {{ notification.message }}
                                        <small class="text-muted fs-6 d-block">
                                            {{ new Date(notification.creeA).toLocaleString() }}
                                        </small>
                                    </li>
                                    <li>
                                        <hr class="dropdown-divider" />
                                    </li>
                                    <li>
                                        <button type="button"
                                                :disabled="nombreNotificationsNonLues < 1"
                                                class="dropdown-item text-center btn btn-danger"
                                                @click="marqueCommeLu">
                                            Marquer toutes comme lues
                                        </button>
                                    </li>
                                </ul>
                            </div>
                            <div class="dropdown text-white" v-if="estConnecte">
                                <a class="nav-link d-flex align-items-center"
                                   role="button"
                                   data-bs-toggle="dropdown"
                                   aria-expanded="false">
                                    <p class="m-0 me-1">{{ utilisateur }}</p>
                                    <img :src="avatarUrl"
                                         alt="Avatar"
                                         class="imgProfile rounded-circle" />
                                </a>
                                <ul class="dropdown-menu dropdown-menu-end bleuMarinFond text-center end-0 top-100">
                                    <li>
                                        <router-link v-if="estClient"
                                                     :to="{ name: 'ModificationProfilUtilisateur' }"
                                                     class="text-decoration-none text-white d-flex align-items-center gap-3">
                                            <a class="dropdown-item text-white btnSurvolerBleuMoyenFond"
                                               @click="activationDropdownProfil = false">
                                                Profil
                                            </a>
                                        </router-link>
                                    </li>
                                    <li>
                                        <a class="dropdown-item text-danger btnSurvolerBleuMoyenFond fw-bold"
                                           href="#"
                                           @click.prevent="deconnecter">
                                            Déconnexion
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </nav>
            <div v-if="
          ($route.name == 'EncanPresent' && ilYAUnEncanPresent == true) ||
          $route.name == 'Encan' ||
          $route.name == 'ResultatRechercheLots'
        ">
                <nav class="navbar bg-white navbarRechercheAvancee">
                    <div class="container-fluid d-flex justify-content-end">
                        <button class="navbar-toggler loupetoggler"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#navbarToggleRechercheAvancee"
                                aria-controls="navbarToggleRechercheAvancee"
                                aria-expanded="false"
                                aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon iconeloupe"></span>
                        </button>
                    </div>
                </nav>
                <div class="bg-white d-flex justify-content-center">
                    <div class="collapse card mt-2 mx-2 mx-md-5 mb-md-5 bg-white col col-lg-9 col-xl-7"
                         id="navbarToggleRechercheAvancee">
                        <div class="card-header d-flex justify-content-center">
                            <h2>{{ titreBarreDeRechercheDeLots }}</h2>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-2">
                                    <label class="text-nowrap recherchelabel fw-bold"
                                           for="rechercheLotsNumeroLot">
                                        Numéro de lot
                                    </label>
                                    <div class="row">
                                        <div class="col-sm">
                                            <input type="number"
                                                   maxlength="10"
                                                   class="form-control rechercheinput align-self-end"
                                                   v-model="rechercheLotsNumeroLot"
                                                   id="rechercheLotsNumeroLot" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-10">
                                    <label class="text-nowrap recherchelabel fw-bold"
                                           for="rechercheLotsValeurEstimee">
                                        Prix estimé
                                    </label>
                                    <div class="row">
                                        <div class="col-lg-3 mb-1 mb-lg-0">
                                            <select class="form-select py-0 selectPourListe"
                                                    v-model="selectValeurEstimee"
                                                    id="selectValeurEstimee"
                                                    aria-label="Default select example">
                                                <option class="py-0" value="0" selected>
                                                    Égale à
                                                </option>
                                                <option class="py-0" value="1">Inférieure à</option>
                                                <option class="py-0" value="2">Supérieure à</option>
                                                <option class="py-0" value="3">Entre</option>
                                            </select>
                                        </div>

                                        <div class="col-sm">
                                            <input type="number"
                                                   maxlength="10"
                                                   class="form-control rechercheinput align-self-end"
                                                   v-model="rechercheLotsValeurEstimee"
                                                   id="rechercheLotsValeurEstimee" />
                                        </div>
                                        <div v-if="selectValeurEstimee == 3"
                                             class="col-sm-auto align-items-center inputAAfficher">
                                            <label class="fs-6">et</label>
                                        </div>
                                        <div v-if="selectValeurEstimee == 3" class="col-sm">
                                            <input type="number"
                                                   maxlength="10"
                                                   class="form-control rechercheinput align-self-end"
                                                   v-model="rechercheLotsValeurEstimee2"
                                                   id="rechercheLotsValeurEstimee2" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-2">
                                <div class="col-md-4">
                                    <label class="text-nowrap recherchelabel fw-bold"
                                           for="rechercheLotsArtiste">
                                        Artiste
                                    </label>
                                    <select class="form-select py-0 align-self-center selectPourListeQuiOntBesoinDeInvalide"
                                            v-model="selectArtiste"
                                            id="selectArtiste"
                                            aria-label="Default select example"
                                            required>
                                        <option class="py-0 pasDeChoix" value="" selected>
                                            Pas de choix
                                        </option>
                                        <option v-for="artiste in listeDesArtistes"
                                                :key="artiste.nomArtiste"
                                                class="py-0"
                                                :value="artiste.nomArtiste">
                                            {{ artiste.nomArtiste }}
                                        </option>
                                    </select>
                                </div>
                                <div class="col-md-4">
                                    <label class="text-nowrap recherchelabel fw-bold"
                                           for="rechercheLotsCategorie">
                                        Catégorie
                                    </label>
                                    <select class="form-select py-0 align-self-center selectPourListeQuiOntBesoinDeInvalide"
                                            v-model="selectCategorie"
                                            id="selectCategorie"
                                            aria-label="Default select example"
                                            required>
                                        <option class="py-0 pasDeChoix" value="" selected>
                                            Pas de choix
                                        </option>
                                        <option v-for="categorie in listeDesCategories"
                                                :key="categorie.id"
                                                class="py-0"
                                                :value="categorie.id">
                                            {{ categorie.nom }}
                                        </option>
                                    </select>
                                </div>
                                <div class="col-md-4">
                                    <label class="text-nowrap recherchelabel fw-bold"
                                           for="rechercheLotsMedium">
                                        Medium
                                    </label>
                                    <select class="form-select py-0 align-self-center selectPourListeQuiOntBesoinDeInvalide"
                                            v-model="selectMedium"
                                            id="selectMedium"
                                            aria-label="Default select example"
                                            required>
                                        <option class="py-0 pasDeChoix" value="" selected>
                                            Pas de choix
                                        </option>
                                        <option v-for="medium in listeDesMediums"
                                                :key="medium.id"
                                                class="py-0"
                                                :value="medium.id">
                                            {{ medium.type }}
                                        </option>
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-6">
                                    <div class="col mt-2">
                                        <label class="text-nowrap recherchelabel fw-bold"
                                               for="rechercheLotsHauteur">
                                            Hauteur
                                        </label>
                                        <div class="row">
                                            <div class="col-lg-5 mb-1 mb-lg-0">
                                                <select class="form-select py-0 selectPourListe"
                                                        v-model="selectHauteur"
                                                        id="selectHauteur"
                                                        aria-label="Default select example">
                                                    <option class="py-0" value="0" selected>
                                                        Égale à
                                                    </option>
                                                    <option class="py-0" value="1">Inférieure à</option>
                                                    <option class="py-0" value="2">Supérieure à</option>
                                                    <option class="py-0"
                                                            value="3"
                                                            id="selectHauteurEntre">
                                                        Entre
                                                    </option>
                                                </select>
                                            </div>
                                            <div class="col-sm">
                                                <input type="number"
                                                       maxlength="10"
                                                       class="form-control rechercheinput align-self-end"
                                                       v-model="rechercheLotsHauteur"
                                                       id="rechercheLotsHauteur" />
                                            </div>
                                            <div v-if="selectHauteur == 3"
                                                 class="col-sm-auto align-items-center inputAAfficher">
                                                <label class="fs-6">et</label>
                                            </div>
                                            <div v-if="selectHauteur == 3" class="col-sm">
                                                <input type="number"
                                                       maxlength="10"
                                                       class="form-control rechercheinput align-self-end"
                                                       v-model="rechercheLotsHauteur2"
                                                       id="rechercheLotsHauteur2" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <div class="col mt-2">
                                        <label class="text-nowrap recherchelabel fw-bold"
                                               for="rechercheLotsLargeur">
                                            Largeur
                                        </label>
                                        <div class="row">
                                            <div class="col-lg-5 mb-1 mb-lg-0">
                                                <select class="form-select py-0 selectPourListe"
                                                        v-model="selectLargeur"
                                                        id="selectLargeur"
                                                        aria-label="Default select example">
                                                    <option class="py-0" value="0" selected>
                                                        Égale à
                                                    </option>
                                                    <option class="py-0" value="1">Inférieure à</option>
                                                    <option class="py-0" value="2">Supérieure à</option>
                                                    <option class="py-0"
                                                            value="3"
                                                            id="selectLargeurEntre">
                                                        Entre
                                                    </option>
                                                </select>
                                            </div>

                                            <div class="col-sm">
                                                <input type="number"
                                                       maxlength="10"
                                                       class="form-control rechercheinput align-self-end"
                                                       v-model="rechercheLotsLargeur"
                                                       id="rechercheLotsLargeur" />
                                            </div>
                                            <div v-if="selectLargeur == 3"
                                                 class="col-sm-auto align-items-center inputAAfficher">
                                                <label class="fs-6">et</label>
                                            </div>
                                            <div v-if="selectLargeur == 3" class="col-sm">
                                                <input type="number"
                                                       maxlength="10"
                                                       class="form-control rechercheinput align-self-end"
                                                       v-model="rechercheLotsLargeur2"
                                                       id="rechercheLotsLargeur2" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col mt-2 mb-2 d-flex justify-content-center">
                            <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                                    type="button"
                                    @click="rechercheAvanceeLots">
                                Lancer la rechercher
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div v-else-if="
          $route.name == 'EncansPasses' ||
          $route.name == 'EncansFuturs' ||
          $route.name == 'TousLesEncans' ||
          $route.name == 'ResultatRechercheEncans'
        ">
                <nav class="navbar bg-white navbarRechercheAvancee">
                    <div class="container-fluid d-flex justify-content-end">
                        <button class="navbar-toggler loupetoggler"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#navbarToggleRechercheAvancee"
                                aria-controls="navbarToggleRechercheAvancee"
                                aria-expanded="false"
                                aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon iconeloupe"></span>
                        </button>
                    </div>
                </nav>
                <div class="bg-white d-flex justify-content-center">
                    <div class="container collapse card mt-2 mx-2 mx-md-5 mb-md-5 bg-white col col-lg-9 col-xl-8"
                         id="navbarToggleRechercheAvancee">
                        <div class="card-header d-flex justify-content-center">
                            <h2>Recherche avancée d'encans</h2>
                        </div>
                        <div class="card-body">
                            <div class="row d-flex flex-row align-items-center">
                                <div class="col-5">
                                    <div class="col mt-2">
                                        <label class="text-nowrap recherchelabel fw-bold"
                                               for="rechercheEncansNumeroEncan">
                                            Numéro d'encan
                                        </label>
                                        <div class="row">
                                            <div class="col-lg-4 mb-1 mb-lg-0">
                                                <select class="form-select py-0 selectPourListe"
                                                        v-model="selectNumeroEncan"
                                                        id="selectNumeroEncan"
                                                        aria-label="Default select example">
                                                    <option class="py-0" value="0" selected>
                                                        Égal à
                                                    </option>
                                                    <option class="py-0"
                                                            id="selectNumeroEncanEntre"
                                                            value="1">
                                                        Entre
                                                    </option>
                                                </select>
                                            </div>

                                            <div class="col-sm">
                                                <input type="number"
                                                       maxlength="10"
                                                       class="form-control rechercheinput align-self-end"
                                                       v-model="rechercheEncansNumeroEncan"
                                                       id="rechercheEncansNumeroEncan" />
                                            </div>
                                            <div v-if="selectNumeroEncan == 1"
                                                 class="col-sm-auto align-items-center inputAAfficher">
                                                <label class="fs-6">et</label>
                                            </div>
                                            <div v-if="selectNumeroEncan == 1" class="col-sm">
                                                <input type="number"
                                                       maxlength="10"
                                                       class="form-control rechercheinput align-self-end"
                                                       v-model="rechercheEncansNumeroEncan2"
                                                       id="rechercheEncansNumeroEncan2" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-7">
                                    <div class="col mt-2">
                                        <label class="text-nowrap recherchelabel fw-bold"
                                               for="rechercheEncansDate">
                                            Date
                                        </label>
                                        <div class="row">
                                            <div class="col-lg-4 d-flex flex-row align-items-center mb-1 mb-lg-0">
                                                <select class="form-select py-0 selectPourListe"
                                                        v-model="selectDate"
                                                        id="selectDate"
                                                        aria-label="Default select example">
                                                    <option class="py-0" value="0" selected>
                                                        Égale à
                                                    </option>
                                                    <option class="py-0" value="1">Inférieure à</option>
                                                    <option class="py-0" value="2">Supérieure à</option>
                                                    <option class="py-0" value="3" id="selectDateEntre">
                                                        Entre
                                                    </option>
                                                </select>
                                            </div>
                                            <VueDatePicker type="date"
                                                           v-model="rechercheEncansDate1"
                                                           id="rechercheEncansDate1"
                                                           class="col-sm"
                                                           :max-date="desacDateDebutEntre"
                                                           :enable-time-picker="false"
                                                           select-text="Choisir"
                                                           cancel-text="Annuler"
                                                           now-button-label="Aujourd'hui"
                                                           :clearable="true"
                                                           :action-row="{ showNow: true }"
                                                           :format-locale="fr"
                                                           :year-range="[2000, new Date().getFullYear() + 10]" />
                                            <div v-if="selectDate == 3"
                                                 class="col-sm-auto align-items-center inputAAfficher">
                                                <label class="fs-6">et</label>
                                            </div>
                                            <VueDatePicker v-if="selectDate == 3"
                                                           type="date"
                                                           v-model="rechercheEncansDate2"
                                                           id="rechercheEncansDate2"
                                                           class="col-sm"
                                                           :min-date="desacDateFinEntre"
                                                           :enable-time-picker="false"
                                                           select-text="Choisir"
                                                           cancel-text="Annuler"
                                                           now-button-label="Aujourd'hui"
                                                           :clearable="true"
                                                           :action-row="{ showNow: true }"
                                                           :format-locale="fr"
                                                           :year-range="[2000, new Date().getFullYear() + 10]" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col mt-2 mb-2 d-flex justify-content-center">
                            <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                                    type="button"
                                    @click="rechercheAvanceeEncans">
                                Lancer la rechercher
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div v-else class="bg-white aucuneBarreDeRechercheAnvancee"></div>
        </div>
    </header>
</template>
<script setup>
    import { computed, watch, ref, onMounted } from "vue";
    import { useStore } from "vuex";
    import { useRouter, useRoute } from "vue-router";
    import VueDatePicker from "@vuepic/vue-datepicker";
    import "@vuepic/vue-datepicker/dist/main.css";
    import { fr } from "date-fns/locale";
    import moment from "moment";

    const store = useStore();
    const router = useRouter();
    const route = useRoute();

    const estConnecte = computed(() => store.state.isLoggedIn);
    const estAdmin = computed(() => store.getters.isAdmin);
    const estClient = computed(() => store.getters.isClient);
    const utilisateur = computed(() => store.getters.username);
    const avatarUrl = computed(() => store.getters.avatarUrl);

    const utilisateurPresent = ref(null);

    const listeDesArtistes = ref([]);
    const listeDesCategories = ref([]);
    const listeDesMediums = ref([]);

    const ilYAUnEncanPresent = ref(false);
    const titreBarreDeRechercheDeLots = ref("");
    var rechercheNumeroEncan = "";
    const rechercheLotsNumeroLot = ref();
    const selectValeurEstimee = ref(0);
    const rechercheLotsValeurEstimee = ref();
    const rechercheLotsValeurEstimee2 = ref();
    const selectArtiste = ref("");
    const selectCategorie = ref("");
    const selectMedium = ref("");
    const selectHauteur = ref(0);
    const rechercheLotsHauteur = ref();
    const rechercheLotsHauteur2 = ref();
    const selectLargeur = ref(0);
    const rechercheLotsLargeur = ref();
    const rechercheLotsLargeur2 = ref();
    const selectNumeroEncan = ref(0);
    const rechercheEncansNumeroEncan = ref();
    const rechercheEncansNumeroEncan2 = ref();
    const selectDate = ref(0);
    const rechercheEncansDate1 = ref();
    const rechercheEncansDate2 = ref();

    watch(
        () => store.state.user,
        (nouvelUtilisateur) => {
            utilisateurPresent.value = nouvelUtilisateur;
        },
        { deep: true, immediate: true }
    );

    const rafraichirInfoUtilisateur = async () => {
        if (estConnecte) {
            try {
                await store.dispatch("fetchClientInfo");
            } catch (error) {
                console.error(
                    "Erreur lors de la récupération des informations client:",
                    error
                );
            }
        }
    };

    // Appelez rafraichirInfoUtilisateur lorsque l'utilisateur se connecte
    watch(
        () => estConnecte,
        (nouvelleValeur) => {
            if (nouvelleValeur) {
                rafraichirInfoUtilisateur();
            }
        }
    );

    var numeroEncanPresent = null;

    async function verifierSiEncanPresent() {
        try {
            const reponse = await store.dispatch("verifierEtatEncan");
            const type = reponse;

            if (type === "courant" || type === "soireeCloture") {
                ilYAUnEncanPresent.value = true;
                const reponseNumEncanCourrant = await store.dispatch(
                    "chercherNumeroEncanEnCours"
                );
                if (reponseNumEncanCourrant.data != "") {
                    numeroEncanPresent = reponseNumEncanCourrant.data;
                }
            } else {
                ilYAUnEncanPresent.value = false;
            }
        } catch (error) {
            console.error("Erreur lors de la vérification de l'encan:", error);
            ilYAUnEncanPresent.value = false;
        }
    }

    const changementDeRoute = computed(() => route.fullPath);
    watch(changementDeRoute, changeTitreBarreRecherche);

    async function changeTitreBarreRecherche() {
        if (route.name == "Encan") {
            rechercheNumeroEncan = route.params.numeroEncan;
        } else if (route.name == "EncanPresent") {
            await verifierSiEncanPresent();
            if (numeroEncanPresent != null) {
                rechercheNumeroEncan = numeroEncanPresent;
            }
        }
        if (titreBarreDeRechercheDeLots) {
            titreBarreDeRechercheDeLots.value =
                "Recherche avancée de lots dans l'Encan " + rechercheNumeroEncan;
        }
        verifierSiQueryDansURL();
    }

    async function rechercheAvanceeLots() {
        var stringquery = {};
        if (route.name == "Encan") {
            rechercheNumeroEncan = route.params.numeroEncan;
        } else if (route.name == "EncanPresent") {
            await verifierSiEncanPresent();
            if (numeroEncanPresent != null) {
                rechercheNumeroEncan = numeroEncanPresent;
            }
        }

        stringquery["numeroEncan"] = rechercheNumeroEncan;
        if (rechercheLotsNumeroLot.value) {
            stringquery["stringNumeroLot"] = rechercheLotsNumeroLot.value;
        }
        if (rechercheLotsValeurEstimee.value) {
            stringquery["selectValeurEstimee"] = selectValeurEstimee.value;
            stringquery["stringValeurEstimee"] = rechercheLotsValeurEstimee.value;
            if (selectValeurEstimee.value == 3) {
                stringquery["stringValeurEstimee2"] = rechercheLotsValeurEstimee2.value;
            }
        }
        if (selectArtiste.value != 0) {
            stringquery["selectArtiste"] = selectArtiste.value;
        }
        if (selectCategorie.value != 0) {
            stringquery["selectCategorie"] = selectCategorie.value;
        }
        if (selectMedium.value != 0) {
            stringquery["selectMedium"] = selectMedium.value;
        }
        if (rechercheLotsHauteur.value) {
            stringquery["selectHauteur"] = selectHauteur.value;
            stringquery["stringHauteur"] = rechercheLotsHauteur.value;
            if (selectHauteur.value == 3) {
                stringquery["stringHauteur2"] = rechercheLotsHauteur2.value;
            }
        }
        if (rechercheLotsLargeur.value) {
            stringquery["selectLargeur"] = selectLargeur.value;
            stringquery["stringLargeur"] = rechercheLotsLargeur.value;
            if (selectLargeur.value == 3) {
                stringquery["stringLargeur2"] = rechercheLotsLargeur.value;
            }
        }

        router.push({
            path: "/resultatrecherchelots",
            query: {
                data: JSON.stringify(stringquery),
            },
        });
    }

    // La barre de recherche avancée garde les informations ou non selon le changement de page
    router.beforeEach((to, from, next) => {
        if (
            from.name == "ResultatRechercheLots" ||
            from.name == "Encan" ||
            from.name == "EncanPresent"
        ) {
            var elementsAEffacer = [
                rechercheLotsNumeroLot,
                rechercheLotsValeurEstimee,
                rechercheLotsValeurEstimee2,
                rechercheLotsHauteur,
                rechercheLotsHauteur2,
                rechercheLotsLargeur,
                rechercheLotsLargeur2,
            ];
            var listeDesSelects = document.querySelectorAll(".selectPourListe");
            var listeDesSelectsQuiOntBesoinDeInvalide = document.querySelectorAll(
                ".selectPourListeQuiOntBesoinDeInvalide"
            );
            if (to.name == "ResultatRechercheLots") {
                if (document.querySelector("#navbarToggleRechercheAvancee")) {
                    document
                        .querySelector("#navbarToggleRechercheAvancee")
                        .classList.remove("show");
                }
                next();
            } else {
                elementsAEffacer.forEach((element) => {
                    if (element) {
                        element.value = "";
                    }
                });
                listeDesSelects.forEach((select) => {
                    if (select) {
                        select.value = 0;
                    }
                });
                listeDesSelectsQuiOntBesoinDeInvalide.forEach((select) => {
                    if (select) {
                        select.value = "";
                    }
                });
                if (document.querySelector("#navbarToggleRechercheAvancee")) {
                    document
                        .querySelector("#navbarToggleRechercheAvancee")
                        .classList.remove("show");
                }
                next();
            }
        } else if (
            from.name == "ResultatRechercheEncans" ||
            from.name == "EncansPasses" ||
            from.name == "EncansFuturs" ||
            from.name == "TousLesEncans"
        ) {
            var elementsAEffacer = [
                rechercheEncansNumeroEncan,
                rechercheEncansNumeroEncan2,
            ];
            var datesAEffacer = [rechercheEncansDate1, rechercheEncansDate2];
            var listeDesSelects = document.querySelectorAll(".selectPourListe");
            if (to.name == "ResultatRechercheEncans") {
                if (document.querySelector("#navbarToggleRechercheAvancee")) {
                    document
                        .querySelector("#navbarToggleRechercheAvancee")
                        .classList.remove("show");
                }
                next();
            } else {
                elementsAEffacer.forEach((element) => {
                    element.value = "";
                });
                datesAEffacer.forEach((date) => {
                    date.value = "";
                });

                listeDesSelects.forEach((select) => {
                    select.value = 0;
                });
                if (document.querySelector("#navbarToggleRechercheAvancee")) {
                    document
                        .querySelector("#navbarToggleRechercheAvancee")
                        .classList.remove("show");
                }
                next();
            }
        } else {
            if (document.querySelector("#navbarToggleRechercheAvancee")) {
                document
                    .querySelector("#navbarToggleRechercheAvancee")
                    .classList.remove("show");
            }
            next();
            changeTitreBarreRecherche();
        }
    });

    const desacDateFinEntre = computed(() => {
        if (rechercheEncansDate1.value) {
            const dateDebutDesac = new Date(rechercheEncansDate1.value);
            dateDebutDesac.setDate(dateDebutDesac.getDate() + 1);

            return [dateDebutDesac];
        }
        return [new Date()];
    });

    const desacDateDebutEntre = computed(() => {
        if (rechercheEncansDate2.value) {
            const dateFinDesac = new Date(rechercheEncansDate2.value);
            dateFinDesac.setDate(dateFinDesac.getDate() - 1);

            return [dateFinDesac];
        }
        return null;
    });

    async function rechercheAvanceeEncans() {
        var stringquery = {};

        if (rechercheEncansNumeroEncan.value) {
            stringquery["selectNumeroEncan"] = selectNumeroEncan.value;
            stringquery["stringNumeroEncan"] = rechercheEncansNumeroEncan.value;
            if (selectNumeroEncan.value == 1) {
                stringquery["stringNumeroEncan2"] = rechercheEncansNumeroEncan2.value;
            }
        }
        if (rechercheEncansDate1.value) {
            var stringDate = moment(rechercheEncansDate1.value).format("yyyy-MM-DD");
            stringquery["selectDate"] = selectDate.value;
            stringquery["stringDate"] = stringDate;
            if (selectDate.value == 3 && rechercheEncansDate2.value) {
                var stringDate2 = moment(rechercheEncansDate2.value).format("yyyy-MM-DD");
                stringquery["stringDate2"] = stringDate2;
            }
        }

        router.push({
            path: "/resultatrechercheencans",
            query: {
                data: JSON.stringify(stringquery),
            },
        });
    }

    function verifierSiQueryDansURL() {
        if (route.query.data != null) {
            var stringquery = JSON.parse(route.query.data);

            if (stringquery.numeroEncan) {
                rechercheNumeroEncan = stringquery.numeroEncan;
                if (titreBarreDeRechercheDeLots) {
                    titreBarreDeRechercheDeLots.value =
                        "Recherche avancée de lots dans l'Encan " + rechercheNumeroEncan;
                }
            }
            if (stringquery.stringNumeroLot) {
                rechercheLotsNumeroLot.value = stringquery.stringNumeroLot;
            }
            if (stringquery.stringValeurEstimee) {
                selectValeurEstimee.value = stringquery.selectValeurEstimee;
                rechercheLotsValeurEstimee.value = stringquery.stringValeurEstimee;
                if (
                    stringquery.selectValeurEstimee == 3 &&
                    stringquery.stringValeurEstimee2
                ) {
                    rechercheLotsValeurEstimee2.value = stringquery.stringValeurEstimee2;
                }
            }
            if (stringquery.selectArtiste != 0 && stringquery.selectArtiste != null) {
                selectArtiste.value = stringquery.selectArtiste;
            }
            if (
                stringquery.selectCategorie != 0 &&
                stringquery.selectCategorie != null
            ) {
                selectCategorie.value = stringquery.selectCategorie;
            }
            if (stringquery.selectMedium != 0 && stringquery.selectMedium != null) {
                selectMedium.value = stringquery.selectMedium;
            }
            if (stringquery.stringHauteur) {
                selectHauteur.value = stringquery.selectHauteur;
                rechercheLotsHauteur.value = stringquery.stringHauteur;
                if (stringquery.selectHauteur == 3 && stringquery.stringHauteur2) {
                    rechercheLotsHauteur2.value = stringquery.stringHauteur2;
                }
            }
            if (stringquery.stringLargeur) {
                selectLargeur.value = stringquery.selectLargeur;
                rechercheLotsLargeur.value = stringquery.stringLargeur;
                if (stringquery.selectLargeur == 3 && stringquery.stringLargeur2) {
                    rechercheLotsLargeur2.value = stringquery.stringLargeur2;
                }
            }
            if (stringquery.stringNumeroEncan) {
                selectNumeroEncan.value = stringquery.selectNumeroEncan;
                rechercheEncansNumeroEncan.value = stringquery.stringNumeroEncan;
                if (
                    stringquery.selectNumeroEncan == 1 &&
                    stringquery.stringNumeroEncan2
                ) {
                    rechercheEncansNumeroEncan2.value = stringquery.stringNumeroEncan2;
                }
            }
            if (stringquery.stringDate) {
                selectDate.value = stringquery.selectDate;
                rechercheEncansDate1.value = moment(stringquery.stringDate);
                if (stringquery.selectDate == 3 && stringquery.stringDate2) {
                    rechercheEncansDate2.value = moment(stringquery.stringDate2);
                }
            }
        }
    }

    // Définition de activationRecherche
    const activationRecherche = ref(false);
    const activationDropdownProfil = ref(false);

    const deconnecter = async () => {
        await store.dispatch("logout");
        router.push("/"); // Redirige vers la page d'accueil après la déconnexion
    };

    onMounted(async () => {
        document.querySelectorAll(".dropdown-toggle").forEach((dropdownToggle) => {
            new bootstrap.Dropdown(dropdownToggle);
        });
        listeDesArtistes.value = await store.dispatch("obtenirArtistes");
        listeDesMediums.value = await store.dispatch("obtenirMediums");
        listeDesCategories.value = await store.dispatch("obtenirCategories");
        verifierSiQueryDansURL();

        const utilisateurId = computed(() => store.state.user?.id);
        if (utilisateurId.value) {
            await store.dispatch("obtenirNotification", utilisateurId.value);
        }
    });

    // Surveiller les changements de route pour mettre à jour l'état
    watch(
        () => route.name,
        async () => {
            if (route.name === "EncanPresent") {
                await verifierSiEncanPresent();
            }
        }
    );

    const notifications = computed(() => {
        return store.state.notifications;
    });
    const nombreNotificationsNonLues = computed(() => {
        return store.state.nombreNotifNonLue;
    });
    console.log("contenu des notif", notifications.value);

    const marqueCommeLu = async function () {
        await store.dispatch("marquerToutesNotifLues");
    };
</script>
<style scoped>
    .imgProfile {
        width: 40px;
        height: 40px;
    }

    input.form-control {
        height: 25px !important;
        font-size: 15px !important;
    }

    select {
        width: 100% !important;
    }

        select.form-select {
            height: 25px !important;
            margin-right: 10px !important;
            font-size: 15px !important;
        }

        select option {
            height: 25px !important;
            font-size: 15px !important;
        }

    recherchelabel {
        margin-left: 10px !important;
        margin-right: 10px !important;
        font-weight: bold !important;
    }

    .rechercheinput {
        margin-right: 10px !important;
    }

    .aucuneBarreDeRechercheAnvancee {
        min-height: 30px;
    }

    .selectwidth {
        width: 160px;
    }

    .inputAAfficher {
        height: 25px;
    }

    .card-body {
        padding-top: 0px !important;
        padding-bottom: 10px !important;
    }

    select,
    select option {
        color: #000000;
    }

        select:invalid,
        select option[value=""] {
            color: #999999;
        }

    .navbarRechercheAvancee {
        padding-top: 3px !important;
        padding-bottom: 2px !important;
        border: none !important;
    }

    .loupetoggler {
        padding-top: 0px !important;
        padding-bottom: 0px !important;
        padding-left: 0px !important;
        padding-right: 0px !important;
        width: 25px;
        height: 25px;
        /*background-image: none;*/
    }

    /*Pour faire changer l'icone du menu hamburger de la recherche*/
    .iconeloupe {
        position: relative !important;
        top: -1px !important;
        width: 20px !important;
        height: 20px !important;
        background-image: url("/icons/Recherche.png") !important;
    }

    .navbar .navbar-collapse .navbar-nav a {
        color: #ffffffa4;
    }

        .navbar .navbar-collapse .navbar-nav a.router-link-active {
            color: #fff;
        }

    /* Ajouter des styles pour indiquer l'état actif */
    .nav-item.nav-link .router-link-active {
        color: #fff !important;
        font-weight: bold;
    }

    .dropdown-item {
        white-space: normal;
    }

    /* Ajuster la taille du badge */
    .badge {
        font-size: 0.8rem;
    }

    .btn-link {
        padding: 0;
    }
</style>
