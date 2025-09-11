<template>
    <div class="d-flex flex-column align-items-center text-center pb-3">
        <h1 id="titreRechercheLots"></h1>
    </div>

    <div class="d-flex gap-2 w-100 justify-content-center" v-if="chargement">
        <div class="spinner-border" role="status">
            <span class="visually-hidden">Chargement des lots...</span>
        </div>
        <p>Chargement des lots en cours...</p>
    </div>

    <div v-else class="w-100">
        <h5 class="text-center" v-if="nbLotsRecus == 0">Aucun lot trouvé</h5>

        <div v-else class="d-flex flex-column align-items-center">
            <div class="d-flex flex-row-reverse w-100 px-4 me-2 gap-2">
                <button class="rounded btn btnSurvolerBleuMoyenFond"
                        v-if="!siTuile"
                        @click="changerTypeAffichage('tuile')">
                    <img src="/icons/TableauOption.png"
                         alt="Affichage en tableau"
                         height="25" />
                </button>
                <button class="rounded btn btnSurvolerBleuMoyenFond"
                        v-else
                        @click="changerTypeAffichage('liste')">
                    <img src="/icons/ListeOption.png"
                         alt="Affichage en liste"
                         height="25" />
                </button>
                <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        type="button"
                        @click="afficherTousLots"
                        v-bind:disabled="lotsParPage == nbLotsRecus">
                    Tous
                </button>
                <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="changerNbLotParPage(100)"
                        v-bind:disabled="lotsParPage == 100">
                    100
                </button>
                <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="changerNbLotParPage(50)"
                        v-bind:disabled="lotsParPage == 50">
                    50
                </button>
                <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="changerNbLotParPage(20)"
                        v-bind:disabled="lotsParPage == 20">
                    20
                </button>
            </div>

            <div v-if="siTuile"
                 class="row row-cols-lg-5 row-cols-md-3 row-cols-sm-2 row-cols-1 w-100 px-3">
                <div v-for="index in lotsAffiche"
                     :key="index.id"
                     class="col p-2 d-flex">
                    <LotTuile :lotRecu="index" />
                </div>
            </div>

            <div v-else class="d-flex flex-column px-5 w-100">
                <div v-for="index in lotsAffiche" :key="index.id" class="p-2">
                    <LotListe :lotRecu="index" />
                </div>
            </div>

            <div class="d-flex flex-row justify-content-center gap-1 flex-wrap p-3">
                <button type="button"
                        class="btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                        @click="reculerPage"
                        v-bind:disabled="pageCourante == 1">
                    ⮜
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
                    ⮞
                </button>
            </div>
        </div>
    </div>
</template>

<script setup>
    import LotTuile from "@/components/views/LotTuile.vue";
    import LotListe from "@/components/views/LotListe.vue";
    import { computed, ref, watch, onMounted } from "vue";
    import { useRouter, useRoute } from "vue-router";
    import { useStore } from "vuex";

    const store = useStore();
    const router = useRouter();
    const route = useRoute();

    const listeLots = ref([]);
    const lots = ref([]);
    var lotsFiltres = ref([]);
    const nbLotsRecus = ref();
    const lotsParPage = ref();
    const listePagination = ref([]);
    const pageCourante = ref(1);
    const siTuile = ref(true);
    const lotsAffiche = ref();
    const nbPages = ref();
    const chargement = ref(true);

    onMounted(async () => {
        try {
            initialiserDonnees();
        } catch (erreur) {
        }
    });

    const queryChangement = computed(() => route.query)
    watch(queryChangement, initialiserDonnees)

    async function initialiserDonnees() {
        const reponse = await store.dispatch("chercherTousLotsRecherche");
        lots.value = reponse.data.map((lot) => ({
            ...lot,
        }));
        genererListeDeLotsFiltree();
        nbLotsRecus.value = lotsFiltres.value.length;
        lotsParPage.value = nbLotsRecus.value;
        nbPages.value = recalculerNbPages();

        genererListePagination();

        chercherLotsAAfficher();
        chargement.value = false;
    }

    function genererListeDeLotsFiltree() {
        lotsFiltres.value = lots.value;
        filtrerLesLotsParNumeroEncan();
        filtrerLesLotsParNumeroLot();
        filtrerLesLotsParValeurEstimee();
        filtrerLesLotsParArtiste();
        filtrerLesLotsParCategorie();
        filtrerLesLotsParMedium();
        filtrerLesLotsParHauteur();
        filtrerLesLotsParLargeur();
    };

    function filtrerLesLotsParNumeroEncan() {
        var stringDeRequete = JSON.parse(route.query.data);
        document.getElementById("titreRechercheLots").innerHTML = "Résultat de la recherche de lots pour l'Encan " + stringDeRequete.numeroEncan;
        lotsFiltres.value = lotsFiltres.value.filter((lot) => {
            if (stringDeRequete.numeroEncan) {
                if (lot.numeroEncan.valueOf() == stringDeRequete.numeroEncan) {
                    return true
                }
                else { return false }
            }
        });
    };

    function filtrerLesLotsParNumeroLot() {
        var stringDeRequete = JSON.parse(route.query.data);
        lotsFiltres.value = lotsFiltres.value.filter((lot) => {
            if (stringDeRequete.stringNumeroLot) {
                if (lot.numero.valueOf() == stringDeRequete.stringNumeroLot) {
                    return true
                }
                else { return false }
            }
            return true
        });
    };

    function filtrerLesLotsParValeurEstimee() {
        var stringDeRequete = JSON.parse(route.query.data);
        lotsFiltres.value = lotsFiltres.value.filter((lot) => {
            if (stringDeRequete.stringValeurEstimee) {
                if (stringDeRequete.selectValeurEstimee == 0) {
                    if (lot.valeurEstimeMin.valueOf() <= stringDeRequete.stringValeurEstimee && lot.valeurEstimeMax.valueOf() >= stringDeRequete.stringValeurEstimee) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectValeurEstimee == 1) {
                    if (lot.valeurEstimeMin.valueOf() <= stringDeRequete.stringValeurEstimee) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectValeurEstimee == 2) {
                    if (lot.valeurEstimeMax.valueOf() >= stringDeRequete.stringValeurEstimee) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectValeurEstimee == 3 && stringDeRequete.stringValeurEstimee2) {
                    if (lot.valeurEstimeMax.valueOf() >= stringDeRequete.stringValeurEstimee && lot.valeurEstimeMin.valueOf() <= stringDeRequete.stringValeurEstimee2) {
                        return true
                    }
                    return false
                }
            }
            return true
        });
    };

    function filtrerLesLotsParArtiste() {
        var stringDeRequete = JSON.parse(route.query.data);
        lotsFiltres.value = lotsFiltres.value.filter((lot) => {
            if (stringDeRequete.selectArtiste != 0 && stringDeRequete.selectArtiste) {
                if (lot.artiste.valueOf() == stringDeRequete.selectArtiste) {
                    return true
                }
                return false
            }
            return true
        });
    };

    function filtrerLesLotsParCategorie() {
        var stringDeRequete = JSON.parse(route.query.data);
        lotsFiltres.value = lotsFiltres.value.filter((lot) => {
            if (stringDeRequete.selectCategorie != 0 && stringDeRequete.selectCategorie) {
                if (lot.idCategorie.valueOf() == stringDeRequete.selectCategorie) {
                    return true
                }
                return false
            }
            return true
        });
    };

    function filtrerLesLotsParMedium() {
        var stringDeRequete = JSON.parse(route.query.data);
        lotsFiltres.value = lotsFiltres.value.filter((lot) => {
            if (stringDeRequete.selectMedium != 0 && stringDeRequete.selectMedium) {
                if (lot.idMedium.valueOf() == stringDeRequete.selectMedium) {
                    return true
                }
                return false
            }
            return true
        });
    };

    function filtrerLesLotsParHauteur() {
        var stringDeRequete = JSON.parse(route.query.data);
        lotsFiltres.value = lotsFiltres.value.filter((lot) => {
            if (stringDeRequete.stringHauteur) {
                if (stringDeRequete.selectHauteur == 0) {
                    if (lot.hauteur.valueOf() == stringDeRequete.stringHauteur) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectHauteur == 1) {
                    if (lot.hauteur.valueOf() <= stringDeRequete.stringHauteur) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectHauteur == 2) {
                    if (lot.hauteur.valueOf() >= stringDeRequete.stringHauteur) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectHauteur == 3 && stringDeRequete.stringHauteur2) {
                    if (lot.hauteur.valueOf() >= stringDeRequete.stringHauteur && lot.hauteur.valueOf() <= stringDeRequete.stringHauteur2) {
                        return true
                    }
                    return false
                }
            }
            return true
        });
    };

    function filtrerLesLotsParLargeur() {
        var stringDeRequete = JSON.parse(route.query.data);
        lotsFiltres.value = lotsFiltres.value.filter((lot) => {
            if (stringDeRequete.stringLargeur) {
                if (stringDeRequete.selectLargeur == 0) {
                    if (lot.largeur.valueOf() == stringDeRequete.stringLargeur) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectLargeur == 1) {
                    if (lot.largeur.valueOf() <= stringDeRequete.stringLargeur) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectLargeur == 2) {
                    if (lot.largeur.valueOf() >= stringDeRequete.stringLargeur) {
                        return true
                    }
                    return false
                }
                if (stringDeRequete.selectLargeur == 3 && stringDeRequete.stringLargeur2) {
                    if (lot.largeur.valueOf() >= stringDeRequete.stringLargeur && lot.largeur.valueOf() <= stringDeRequete.stringLargeur2) {
                        return true
                    }
                    return false
                }
            }
            return true
        });
    };

    watch(pageCourante, () => {
        genererListePagination();
        chercherLotsAAfficher();
    });

    const changerTypeAffichage = ref(function (typeAffichage) {
        if (typeAffichage == "tuile") {
            siTuile.value = true;
        } else {
            siTuile.value = false;
        }
        pageCourante.value = 1;
        genererListePagination();
        chercherLotsAAfficher();
    });

    const changerNbLotParPage = ref(function (nouvLotsParPage) {
        lotsParPage.value = nouvLotsParPage;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        genererListePagination();
        chercherLotsAAfficher();
    });

    const afficherTousLots = ref(function () {
        lotsParPage.value = nbLotsRecus.value;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        genererListePagination();
        chercherLotsAAfficher();
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
        return Math.ceil(nbLotsRecus.value / lotsParPage.value);
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

    function chercherLotsAAfficher() {
        lotsAffiche.value = [];

        let positionDebut = (pageCourante.value - 1) * lotsParPage.value;
        let positionFin = pageCourante.value * lotsParPage.value;

        for (
            let i = positionDebut;
            i < positionFin && i < lotsFiltres.value.length;
            i++
        ) {
            lotsAffiche.value.push(lotsFiltres.value[i]);
        }
    }
</script>

<style scoped></style>
