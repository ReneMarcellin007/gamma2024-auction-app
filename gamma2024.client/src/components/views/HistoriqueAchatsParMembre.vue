<template>
    <span v-for="encan in numerosEncans">
        <FactureModal :idFacture="encan.id"></FactureModal>
        <FactureLivraisonModal v-if="encan.livraison == true" :idFacture="encan.idFactureLivraison"></FactureLivraisonModal>
    </span>
    <div class="container mt-5">
        <router-link class="text-decoration-none" :to="{ name: 'ChoixLivraison', params: {idFacture:facture.id} }" v-for="facture in listeFacturesChoixAFaire" :key="facture.id">
            <div class="alert alert-danger d-flex flex-row gap-1" role="alert">
                <p class="m-0">Veuillez faire votre choix de livraison pour l'encan {{ facture.numeroEncan }} : </p>
                <p class="text-decoration-underline m-0">Payez la facture ici!</p>
            </div>
        </router-link>

        <h3 class="text-center">Rechercher un achat</h3>

        <input v-model="rechercheQuery"
               class="form-control row col-10 ms-1"
               type="search"
               placeholder="Rechercher un achat"
               aria-label="Search" />

        <h1 class="text-center mt-5">Liste des achats</h1>

        <div class="d-flex flex-row gap-2 justify-content-md-end justify-content-center mb-3">
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

        <div class="d-flex gap-2 justify-content-center" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des ventes...</span>
            </div>
            <p>Chargement des ventes en cours...</p>
        </div>

        <div v-if="!chargement" class="w-100">
            <div class="d-flex justify-content-center" v-if="!ventesFiltrees.length">
                <h2>Aucun résultat trouvé</h2>
            </div>

            <div v-else class="mb-5">
                <div class="accordion" id="accordionEncan">
                    <div class="accordion-item" v-for="encan in numerosEncans" :key="encan">
                        <div v-if="ventesFiltrees.filter((x) => x.encan == encan.encan) != 0 ">
                            <h2 class="accordion-header px-0 d-flex" :style="{border: styleBordure}">
                                <button class="accordion-button" type="button" data-bs-toggle="collapse" :data-bs-target="'#collapse' + encan.encan"
                                        aria-expanded="true" :aria-controls="'collapse' + encan.encan">
                                    <div class="d-flex align-items-center col-10">
                                        <div class="me-3">
                                            <img v-if="encan.livraison == true"
                                                 src="/icons/Livrable.png"
                                                 height="30"
                                                 width="30" />
                                            <img v-else-if="encan.livraison == false"
                                                 src="/icons/Cueillette.png"
                                                 height="30"
                                                 width="30" />
                                            <img v-else
                                                 src="/icons/Avertissement.png"
                                                 height="30"
                                                 width="30" />
                                        </div>

                                        <div class="d-flex flex-md-row flex-column">
                                            <div class="pe-5">
                                                Encan : {{ encan.encan }}
                                            </div>
                                            <div>
                                                Date de la facturation : {{ encan.dateAchat.split("T")[0] }}
                                            </div>
                                        </div>

                                    </div>

                                    <div class="d-flex flex-column flex-md-row col justify-content-md-end me-md-2">

                                        <button v-if="encan.livraison == true"
                                                class="btn btn-info mb-2 mb-md-0 me-md-2"
                                                data-bs-toggle="modal"
                                                :data-bs-target="'#modalFactureLivraison'+encan.idFactureLivraison">
                                            <img src="/icons/Livrable.png"
                                                 height="30"
                                                 width="30" alt="..." />
                                        </button>
                                        <button class="btn btn-info"
                                                data-bs-toggle="modal"
                                                :data-bs-target="'#modalFacture'+encan.id">
                                            <img src="/icons/VoirBtn.png"
                                                 height="30"
                                                 width="30" alt="..." />
                                        </button>
                                    </div>
                                </button>
                            </h2>
                            <!--Changer le 233 en le numero de l'encan le plus récent-->
                            <div :id="'collapse' + encan.encan"
                                 class="accordion-collapse collapse"
                                 :class="{ show: encan == numerosEncans[0] }"
                                 data-bs-parent="#accordionEncan">
                                <div class="accordion-body">
                                    <div class="accordion" id="accordionClient" v-for="facture in ventesAffichees.filter((x)=> x.encan == encan.encan)" :key="facture.id">
                                        <table class="table text-center">
                                            <thead class="table-dark">
                                                <tr>
                                                    <th scope="col">Numéro du lot</th>
                                                    <th scope="col">Prix vendu</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr v-for="lot in facture.lots" :key="lot.id">
                                                    <td scope="row">{{ lot.numero }}</td>
                                                    <td>{{ lot.mise }}$</td>
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

        <div class="d-flex flex-row justify-content-center gap-1 flex-wrap p-3" v-if="ventesAffichees.length != 0">
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
</template>

<script setup>
    import { computed, watch, onMounted, ref } from "vue";
    import { useStore } from "vuex";
    import FactureModal from "@/components/modals/VoirFactureModal.vue";
    import FactureLivraisonModal from "@/components/modals/VoirFactureLivraisonModal.vue";

    const store = useStore();
    const listeFacturesMembre = ref([]);
    const numerosEncans = ref([]);
    const rechercheQuery = ref("");
    const styleBordure = ref('');
    const chargement = ref(true);

    const pageCourante = ref(1);
    const ventesParPage = ref();
    const nbVentesRecus = ref();
    const nbPages = ref();
    const listePagination = ref([]);
    const ventesAffichees = ref([]);

    const listeFacturesChoixAFaire = ref([]);

    onMounted(async () => {
        try {
            listeFacturesMembre.value = await store.dispatch("fetchFactureInfoMembre");

            listeFacturesMembre.value.filter(function (value, index, self) {
                if ((index === self.findIndex((t) => t.encan === value.encan)) == true) {
                    numerosEncans.value.push(value);
                }
            });

            const reponse = await store.dispatch("chercherFacturesChoixAFaire");
            listeFacturesChoixAFaire.value = reponse.data;

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
        if (listeFacturesMembre.value) {
            return listeFacturesMembre.value;
        }
        return [];
    });

    const ventesFiltrees = computed(() => {
        return tousLesVentes.value.filter((vente) => {
            const rechercheEnMinuscule = rechercheQuery.value.toLowerCase();
            return (
                vente.encan.toString().startsWith(rechercheEnMinuscule) ||
                vente.dateAchat.toLowerCase().startsWith(rechercheEnMinuscule) ||
                vente.lots.find(l => l.numero.startsWith(rechercheEnMinuscule))
            );
        });
    });

    watch(ventesFiltrees, () => {
        if (listeFacturesMembre.value.length == ventesFiltrees.value.length) {
            styleBordure.value = 'none';
        }
        else {
            styleBordure.value = '2px solid green'
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

        for (let i = positionDebut; i < positionFin && i < ventesFiltrees.value.length; i++) {
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
        font-size: 16px;
    }

    .btnVisuel {
        background-color: #0dcaf0;
    }

    .accordion {
        --bs-accordion-active-bg: none;
    }

    .h-52 {
        height: 52px;
    }
</style>