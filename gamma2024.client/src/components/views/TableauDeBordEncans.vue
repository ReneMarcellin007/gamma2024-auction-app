<template>
    <div class="container">
        <h1 class="text-center mb-2 mb-md-5">Liste des encans</h1>

        <h3 class="text-center" for="Recherche">Rechercher un encan </h3>

        <div class="d-flex flex-column gap-3 align-items-center mb-5">
            <input data-bs-theme="light"
                   type="search"
                   aria-label="RechercheNum"
                   v-model="encanRechercheNumEncan"
                   placeholder="Rechercher par numéro encan"
                   class="form-control" />

            <input data-bs-theme="light"
                   type="search"
                   aria-label="RechercheDate"
                   v-model="encanRechercheDate"
                   placeholder="Rechercher par date (AAAA-MM-JJ)"
                   class="form-control" />
        </div>


        <router-link :to="{ name: 'TableauDeBordEncansAjout' }"
                     class="text-decoration-none">
            <button class="btn fs-5 btn-block w-100 btnSurvolerBleuMoyenFond btnClick text-white"
                    type="button"
                    id="ajouterEncanButton">
                Ajouter un encan
            </button>
        </router-link>

        <transition name="fade">
            <div v-if="messageErreur" class="alert alert-danger">
                {{ messageErreur }}
            </div>
            <div v-else>
                <div v-if="messageSucces" class="alert alert-success">
                    {{ messageSucces }}
                </div>
            </div>
        </transition>

        <div class="d-flex gap-2 justify-content-center mt-4" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des encans...</span>
            </div>
            <p>Chargement des encans en cours...</p>
        </div>

        <div v-if="!chargement" class="w-100">
            <div class="d-flex justify-content-center justify-content-md-end my-4" v-if="listeEncansFiltree.length">
                <div class="d-flex flex-row gap-2">
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbEncanParPage(20)"
                            v-bind:disabled="encansParPage == 20">
                        20
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbEncanParPage(50)"
                            v-bind:disabled="encansParPage == 50">
                        50
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbEncanParPage(100)"
                            v-bind:disabled="encansParPage == 100">
                        100
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            type="button"
                            @click="afficherTousEncans"
                            v-bind:disabled="encansParPage == nbEncansRecus">
                        Tous
                    </button>
                </div>
            </div>

            <div class="d-flex justify-content-center mt-4" v-if="!listeEncansFiltree.length">
                <h2>Aucun résultat trouvé</h2>
            </div>

            <div v-else class="overflow-auto">
                <table class="table table-striped mx-0 text-center">
                    <thead class="table-dark">
                        <tr>
                            <th data-field="numeroEncan">Encan</th>
                            <th data-field="statut">Statut</th>
                            <th data-field="dateDebut">Date de début</th>
                            <th data-field="dateFin">Date de fin</th>
                            <th data-field="soireeCloture">Soirée de clôture</th>
                            <th data-field="nbLot">Nombre de lots</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="encan in encansAffiche" :key="encan.id">
                            <td class="align-middle">{{ encan.numeroEncan }}</td>
                            <td class="align-middle">
                                <div class="d-flex justify-content-center collapse dropdown dropdown-center">
                                    <button :encanId="encan.id"
                                            class="btn dropdown-toggle bleuMarinSecondaireFond rounded text-white contenuListeDropdown fs-7"
                                            type="button"
                                            data-bs-toggle="dropdown"
                                            aria-expanded="false">
                                        <span v-if="encan.estPublie == true">Publié</span>
                                        <span v-else>Non publié</span>
                                    </button>

                                    <ul class="dropdown-menu dropdown-menu-dark bleuMarinFond text-center">
                                        <li>
                                            <a class="dropdown-item"
                                               @click="encanPublieMAJ(true)"
                                               :encanId="encan.id">Publié</a>
                                        </li>
                                        <li>
                                            <a class="dropdown-item"
                                               @click="encanPublieMAJ(false)"
                                               :encanId="encan.id">Non publié</a>
                                        </li>
                                    </ul>
                                </div>
                            </td>
                            <td class="align-middle">{{ encan.dateDebut.split("T")[0] }}</td>
                            <td class="align-middle">{{ encan.dateFin.split("T")[0] }}</td>
                            <td class="align-middle">
                                {{ encan.dateDebutSoireeCloture.split("T")[0] }}
                                {{ encan.dateDebutSoireeCloture.split("T")[1].split(".")[0] }}
                            </td>
                            <td class="align-middle">{{ encan.nbLots }}</td>
                            <td class="align-middle">
                                <div v-if="VerifEstPasse(encan.dateDebut)" class="d-flex flex-row justify-content-center">
                                    <button class="btn bleuMarinSecondaireFond px-3 me-3 btnModifierIcone" @click="editerEncan(encan.id)">
                                        <img src="/icons/ModifierBtn.png"
                                             width="30"
                                             height="30"
                                             alt="..." />
                                    </button>
                                    <button class="btn btn-danger px-3 btn_delete"
                                            data-bs-toggle="modal"
                                            :data-bs-target="'#' + encan.numeroEncan">
                                        <img src="/icons/SupprimerBtn.png"
                                             width="30"
                                             height="30"
                                             alt="..." />
                                    </button>
                                </div>
                            </td>
                            <ConfirmDelete :h="encan" @supprimerEncan="supprimerMonEncan" />
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="d-flex flex-row justify-content-center gap-1 flex-wrap p-3" v-if="listeEncansFiltree.length">
                <button type="button"
                        class="btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="reculerPage"
                        v-bind:disabled="pageCourante == 1">
                    <
                </button>

                <div v-for="item in listePagination">
                    <button type="button"
                            class="btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            :pageId="item"
                            @click="changerPage()"
                            v-bind:disabled="pageCourante == item || item == '...'">
                        {{ item }}
                    </button>
                </div>

                <button type="button"
                        class="btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="avancerPage"
                        v-bind:disabled="pageCourante == nbPages">
                    >
                </button>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { onMounted, ref, watch, reactive } from "vue";
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";
    import ConfirmDelete from "@/components/modals/ConfirmDeleteEncan.vue";

    const store = useStore();
    const router = useRouter();
    const listeEncans = ref([]);
    const listeEncansFiltree = ref([]);
    const nbEncansRecus = ref();
    const encansParPage = ref();
    const listePagination = ref([]);
    const pageCourante = ref(1);
    const nbPages = ref();
    const encansAffiche = ref([]);

    let encanPublieMAJ;
    const encanRechercheNumEncan = ref();
    const encanRechercheDate = ref();

    const messageErreur = ref("");
    const messageSucces = ref("");

    const encan = ref("");
    const chargement = ref(true);

    onMounted(async () => {
        try {
            initialiserDonnees();
        } catch (erreur) {
            console.error("Erreur encans" + erreur);
        }
    });

    const supprimerMonEncan = async (idEncan) => {
        await store.dispatch("supprimerUnEncan", idEncan);
        initialiserDonnees();
    };

    const editerEncan = async (idEncan) => {
        router.push({ name: "ModificationEncan", params: { id: idEncan } });
    };

    watch(encanRechercheNumEncan, () => {
        listeEncansFiltree.value = listeEncans.value;

        listeEncansFiltree.value = listeEncansFiltree.value.filter(
            ({ numeroEncan }) =>
                numeroEncan.toString().startsWith(encanRechercheNumEncan.value)
        );

        nbEncansRecus.value = listeEncansFiltree.value.length;
        pageCourante.value = 1;
        AjusterPagination();
    });

    watch(pageCourante, () => {
        genererListePagination();
        chercherEncansAAfficher();
    });

    watch(encanRechercheDate, () => {
        listeEncansFiltree.value = listeEncans.value;
        const rechercheDate = new Date(encanRechercheDate.value);

        listeEncansFiltree.value = listeEncansFiltree.value.filter(
            ({ dateDebut, dateFin, dateDebutSoireeCloture }) => {
                const dateDebutObj = new Date(dateDebut);
                const dateFinObj = new Date(dateFin);

                let listeEncanRechecheDate = encanRechercheDate.value.split("-")

                if (listeEncanRechecheDate.length == 3 && !listeEncanRechecheDate.includes("")) {
                    return (
                        dateDebut.toString().startsWith(encanRechercheDate.value) ||
                        dateFin.toString().startsWith(encanRechercheDate.value) ||
                        dateDebutSoireeCloture.startsWith(encanRechercheDate.value) ||
                        (rechercheDate >= dateDebutObj && rechercheDate <= dateFinObj)
                    );
                }
                else {
                    return (
                        dateDebut.toString().startsWith(encanRechercheDate.value) ||
                        dateFin.toString().startsWith(encanRechercheDate.value) ||
                        dateDebutSoireeCloture.startsWith(encanRechercheDate.value)
                    );
                }
            }
        );

        nbEncansRecus.value = listeEncansFiltree.value.length;
        pageCourante.value = 1;
        AjusterPagination();
    });

    const changerNbEncanParPage = ref(function (nouvEncansParPage) {
        encansParPage.value = nouvEncansParPage;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        AjusterPagination();
    });

    const afficherTousEncans = ref(function () {
        encansParPage.value = nbEncansRecus.value;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        AjusterPagination();
    });

    const reculerPage = ref(function () {
        if (pageCourante.value > 1) {
            pageCourante.value--;
        }
    });

    const avancerPage = ref(function () {
        if (pageCourante.value < nbPages.value) {
            pageCourante.value++;
        }
    });

    const changerPage = ref(function () {
        pageCourante.value = parseInt(event.srcElement.getAttribute("pageId"));
    });

    function recalculerNbPages() {
        return Math.ceil(nbEncansRecus.value / encansParPage.value);
    }

    function genererListePagination() {
        listePagination.value = [];

        for (let i = 1; i <= nbPages.value; i++) {
            if (
                i == 1 ||
                (i >= pageCourante.value - 1 && i <= pageCourante.value + 1) ||
                i == nbPages.value
            ) {
                listePagination.value.push(i);
            } else if (
                listePagination.value[listePagination.value.length - 1] != "..."
            ) {
                listePagination.value.push("...");
            }
        }
    }

    function chercherEncansAAfficher() {
        encansAffiche.value = [];

        let positionDebut = (pageCourante.value - 1) * encansParPage.value;
        let positionFin = pageCourante.value * encansParPage.value;

        for (
            let i = positionDebut;
            i < positionFin && i < listeEncansFiltree.value.length;
            i++
        ) {
            encansAffiche.value.push(listeEncansFiltree.value[i]);
        }
    }

    async function initialiserDonnees() {
        listeEncans.value = await store.dispatch("fetchEncanInfo");

        listeEncansFiltree.value = listeEncans.value;

        nbEncansRecus.value = listeEncansFiltree.value.length;
        encansParPage.value = nbEncansRecus.value;
        nbPages.value = recalculerNbPages();

        genererListePagination();
        chercherEncansAAfficher();

        encanPublieMAJ = async function (statutPublie) {
            let encanId = event.srcElement.getAttribute("encanId");
            encan.value = listeEncans.value.find((e) => e.id == encanId);

            if (encan.value.estPublie != statutPublie) {
                encan.value.estPublie = !encan.value.estPublie;

                let formData = reactive({
                    numeroEncan: encan.value.numeroEncan,
                    estPublie: encan.value.estPublie,
                });

                const reponse = await store.dispatch("mettreAJourEncanPublie", formData);
                if (reponse.success) {
                    messageSucces.value = "Statut encan modifié!";
                    messageErreur.value = "";
                    setTimeout(() => {
                        messageSucces.value = "";
                    }, 5000);
                } else {
                    messageErreur.value = reponse.error;
                    messageSucces.value = "";
                    setTimeout(() => {
                        messageErreur.value = "";
                    }, 5000);
                }
            }
        };

        chargement.value = false;
    }

    function AjusterPagination() {
        nbPages.value = recalculerNbPages();
        genererListePagination();
        chercherEncansAAfficher();
    }

    function VerifEstPasse(dDebut) {
        const maintenant = new Date();

        const debut = new Date(dDebut);

        if (debut >= maintenant) {
            return true;
        }
        else {
            return false;
        }
    }
</script>

<style scoped>
    .dropdown-toggle[aria-expanded="true"] {
        background-color: #5a708a;
    }

    .dropdown-item:active {
        background-color: #5a708a;
    }

    .btn_delete {
        background-color: rgb(194, 8, 8);
    }

        .btn_delete:hover {
            background-color: rgb(235, 6, 6);
        }

    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 1s ease;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }

    table,
    input {
        box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.2), 0 3px 5px 0 rgba(0, 0, 0, 0.19);
    }

    th {
        font-size: 18px;
    }

    td {
        font-size: 14px;
    }
</style>
