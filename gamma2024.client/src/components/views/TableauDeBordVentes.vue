<template>
    <span v-for="facture in ventesAffichees" :key="facture.id">
        <FactureModal :idFacture="facture.id"></FactureModal>
        <FactureLivraisonModal v-if="facture.livraison == true" :idFacture="facture.idFactureLivraison"></FactureLivraisonModal>
    </span>
    <div class="container">
        <h1 class="text-center mb-2 mb-md-5">Liste des ventes</h1>

        <h3 class="text-center">Rechercher une vente</h3>

        <input v-model="requeteRecherche"
               class="form-control row col-10 ms-1"
               type="search"
               placeholder="Rechercher une vente"
               aria-label="Search" />

        <div class="d-flex gap-2 justify-content-center mt-4" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des ventes...</span>
            </div>
            <p>Chargement des ventes en cours...</p>
        </div>

        <div v-if="!chargement" class="w-100">
            <div class="d-flex flex-row gap-2 justify-content-center justify-content-md-end my-4" v-if="ventesFiltrees.length">
                <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="changerNbVenteParPage(20)"
                        v-bind:disabled="ventesParPage == 20">
                    20
                </button>
                <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="changerNbVenteParPage(50)"
                        v-bind:disabled="ventesParPage == 50">
                    50
                </button>
                <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="changerNbVenteParPage(100)"
                        v-bind:disabled="ventesParPage == 100">
                    100
                </button>
                <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        type="button"
                        @click="afficherTousVentes"
                        v-bind:disabled="ventesParPage == nbVentesRecus">
                    Tous
                </button>
            </div>

            <div class="d-flex justify-content-center mt-4" v-if="!ventesFiltrees.length">
                <h2>Aucun résultat trouvé</h2>
            </div>

            <div v-else>
                <div class="accordion" id="accordionEncan">
                    <div class="accordion-item" v-for="encan in numerosEncans" :key="encan">
                        <div v-if="ventesFiltrees.filter((x) => x.encan == encan.encan) != 0">
                            <h2 class="accordion-header px-0" :style="{border: styleDeBordure}">
                                <button class="accordion-button d-flex flex-column flex-md-row align-items-start"
                                        type="button"
                                        data-bs-toggle="collapse"
                                        :data-bs-target="'#collapse' + encan.encan"
                                        aria-expanded="true"
                                        :aria-controls="'collapse' + encan.encan">
                                    <div class="pe-5">
                                        Encan : {{ encan.encan }}
                                    </div>
                                    <div>
                                        Date de la facturation : {{ encan.dateAchat.split("T")[0] }}
                                    </div>
                                </button>
                            </h2>

                            <div :id="'collapse' + encan.encan"
                                 class="accordion-collapse collapse"
                                 :class="{ show: encan == numerosEncans[0] }"
                                 data-bs-parent="#accordionEncan">
                                <div class="accordion-body">
                                    <div class="accordion" id="accordionClient" v-for="facture in ventesAffichees.filter((x)=> x.encan == encan.encan)" :key="facture.id">
                                        <div class="accordion-item">
                                            <h2 class="accordion-header px-0">
                                                <button class="accordion-button d-flex flex-wrap"
                                                        type="button"
                                                        data-bs-toggle="collapse"
                                                        :data-bs-target="'#collapseFacture' + facture.id"
                                                        aria-expanded="true"
                                                        :aria-controls="'collapseFacture' + facture.id">
                                                    <div class="d-flex col-8 align-items-center">
                                                        <div class="me-2">
                                                            <img v-if="facture.livraison == true"
                                                                 src="/icons/Livrable.png"
                                                                 height="30" width="30" />
                                                            <img v-else-if="facture.livraison == false"
                                                                 src="/icons/Cueillette.png"
                                                                 height="30" width="30" />
                                                            <img v-else
                                                                 src="/icons/Minuteur.png"
                                                                 height="30" width="30" />
                                                        </div>
                                                        <div>
                                                            {{ facture.prenom }} {{ facture.nom }} ({{facture.pseudonyme}})<br />{{ facture.courriel }}<br />{{facture.telephone}}
                                                        </div>

                                                    </div>
                                                    <div class="d-flex flex-column flex-md-row col-3 justify-content-md-end align-items-end align-items-md-center">
                                                        <div v-if="facture.livraison == true" class="mb-2 mb-md-0 me-md-2">
                                                            <button class="btn btn-info"
                                                                    data-bs-toggle="modal"
                                                                    :data-bs-target="'#modalFactureLivraison'+facture.idFactureLivraison">
                                                                <img src="/icons/Livrable.png" class="btnVisuel" height="30" width="30" alt="..." />
                                                            </button>
                                                        </div>
                                                        <div class="me-md-2">
                                                            <button class="btn btn-info"
                                                                    data-bs-toggle="modal"
                                                                    :data-bs-target="'#modalFacture'+facture.id">
                                                                <img src="/icons/VoirBtn.png" class="btnVisuel" height="30" width="30" alt="..." />
                                                            </button>
                                                        </div>

                                                    </div>
                                                </button>
                                            </h2>
                                            <div :id="'collapseFacture' + facture.id"
                                                 class="accordion-collapse collapse"
                                                 data-bs-parent="#accordionClient">
                                                <div class="accordion-body">
                                                    <table class="table text-center">
                                                        <thead class="table-dark">
                                                            <tr>
                                                                <th scope="col">Numéro du lot</th>
                                                                <th scope="col">Prix vendu</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr v-for="lot in facture.lots" :key="lot.id">
                                                                <td class="align-middle" scope="row">{{ lot.numero }}</td>
                                                                <td class="align-middle">{{ lot.mise }}$</td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="d-flex flex-row justify-content-center gap-1 flex-wrap p-3" v-if="ventesAffichees.length">
                <button type="button"
                        class="btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="reculerPage"
                        v-bind:disabled="pageCourante == 1">
                    <
                </button>

                <div v-for="item in listePagination" :key="item.id">
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
    import { computed, watch, onMounted, ref } from "vue";
    import { useStore } from "vuex";
    import FactureModal from "@/components/modals/VoirFactureModal.vue";
    import FactureLivraisonModal from "@/components/modals/VoirFactureLivraisonModal.vue";

    const store = useStore();
    const listeFactures = ref([]);
    const requeteRecherche = ref("");
    const numerosEncans = ref([]);
    const styleDeBordure = ref('');
    const chargement = ref(true);

    const pageCourante = ref(1);
    const ventesParPage = ref();
    const nbVentesRecus = ref();
    const nbPages = ref();
    const listePagination = ref([]);
    const ventesAffichees = ref([]);

    onMounted(async () => {
        try {
            listeFactures.value = await store.dispatch("fetchFactureInfo");

            listeFactures.value.filter(function (value, index, self) {
                if ((index === self.findIndex((t) => t.encan === value.encan)) == true) {
                    numerosEncans.value.push(value);
                }
            });

            nbVentesRecus.value = ventesFiltrees.value.length;
            ventesParPage.value = nbVentesRecus.value;
            nbPages.value = recalculerNbPages();

            genererListePagination();
            chercherVentesAAfficher();

            chargement.value = false;
        } catch (erreur) {
            console.error("Erreur factures" + erreur);
        }
    });

    const tousLesVentes = computed(() => {
        if (listeFactures.value) {
            return listeFactures.value;
        }
        return [];
    });

    const ventesFiltrees = computed(() => {
        return tousLesVentes.value.filter((vente) => {
            const rechercheEnMinuscule = requeteRecherche.value.toLowerCase();
            return (
                vente.encan.toString().startsWith(rechercheEnMinuscule) ||
                vente.dateAchat.toLowerCase().startsWith(rechercheEnMinuscule) ||
                vente.prenom.toLowerCase().startsWith(rechercheEnMinuscule) ||
                vente.nom.toLowerCase().startsWith(rechercheEnMinuscule) ||
                vente.pseudonyme.toLowerCase().startsWith(rechercheEnMinuscule) ||
                vente.courriel.toLowerCase().startsWith(rechercheEnMinuscule) ||
                vente.telephone.toLowerCase().startsWith(rechercheEnMinuscule)
            );
        });
    });

    watch(ventesFiltrees, () => {
        if (listeFactures.value.length == ventesFiltrees.value.length) {
            styleDeBordure.value = 'none';
        }
        else {
            styleDeBordure.value = '2px solid green'
        }

        nbVentesRecus.value = ventesFiltrees.value.length;
        pageCourante.value = 1;
        AjusterPagination();
    });

    watch(pageCourante, () => {
        genererListePagination();
        chercherVentesAAfficher();
    });

    const changerNbVenteParPage = ref(function (nouvVentesParPage) {
        ventesParPage.value = nouvVentesParPage;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        AjusterPagination();
    });

    const afficherTousVentes = ref(function () {
        ventesParPage.value = nbVentesRecus.value;
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
        return Math.ceil(nbVentesRecus.value / ventesParPage.value);
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

    function AjusterPagination() {
        nbPages.value = recalculerNbPages();
        genererListePagination();
        chercherVentesAAfficher();
    }

    function chercherVentesAAfficher() {
        ventesAffichees.value = [];

        let positionDebut = (pageCourante.value - 1) * ventesParPage.value;
        let positionFin = pageCourante.value * ventesParPage.value;

        for (
            let i = positionDebut;
            i < positionFin && i < ventesFiltrees.value.length;
            i++
        ) {
            ventesAffichees.value.push(ventesFiltrees.value[i]);
        }
    }
</script>

<style scoped>

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

    .btnVisuel {
        background-color: #0dcaf0;
    }

    .accordion {
        --bs-accordion-active-bg: none;
    }
</style>
