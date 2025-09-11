<template>
    <div class="d-flex gap-2 w-100 justify-content-center" v-if="chargement">
        <div class="spinner-border" role="status">
            <span class="visually-hidden">Chargement des lots...</span>
        </div>
        <p>Chargement des lots en cours...</p>
    </div>

    <div v-else class="w-100">

        <h5 class="text-center" v-if="nbLotsRecus == 0">
            Aucun lot trouvé
        </h5>

        <div v-else class="d-flex flex-column align-items-center">

            <div class="d-flex flex-row-reverse w-100 px-4 me-2 gap-2 ">
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
                    <img src="/icons/ListeOption.png" alt="Affichage en liste" height="25" />
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
                <div v-for="index in lotsAffiches"
                     :key="index.id"
                     class="col p-2 d-flex">
                    <LotTuile :lotRecu="index" />
                </div>
            </div>

            <div v-else class="d-flex flex-column px-5 w-100">
                <div v-for="index in lotsAffiches" :key="index.id" class="p-2">
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
    import { ref, watch, onMounted, computed } from "vue";
    import { useStore } from "vuex";

    const store = useStore();

    const props = defineProps({
        idEncan: Number,
    });

    const listeLots = ref([]);
    const nbLotsRecus = ref();
    const lotsParPage = ref();
    const listePagination = ref([]);
    const pageCourante = ref(1);
    const siTuile = ref(true);
    const lotsAffiches = ref();
    const nbPages = ref();
    const chargement = ref(true);

    // Ajouter un computed pour les lots du store
    const storeLots = computed(() => store.state.lots);

    // Surveiller les changements dans le store
    watch(storeLots, () => {
        // Mettre à jour la liste des lots affichés si nécessaire
        if (storeLots.value.length > 0) {
            listeLots.value = storeLots.value;
            chercherLotsAAfficher();
        }
    }, { deep: true });

    onMounted(async () => {
        try {
            const reponse = await store.dispatch(
                "chercherTousLotsParEncan",
                props.idEncan
            );

            if (reponse && reponse.data) {
                listeLots.value = reponse.data;

                listeLots.value = listeLots.value.sort(function (x, y) {
                    var xNb = parseInt(x.numero.replace(/[^0-9]+/g, ""));
                    var yNb = parseInt(y.numero.replace(/[^0-9]+/g, ""));

                    if (xNb == yNb) {
                        var xLtr = x.numero.replace(/[^A-Za-z]+/g, "");
                        var yLtr = y.numero.replace(/[^A-Za-z]+/g, "");

                        return xLtr.localeCompare(yLtr)
                    }
                    else if (xNb < yNb) {
                        return -1;
                    }
                    else {
                        return 1;
                    }
                })


                nbLotsRecus.value = listeLots.value.length;
                lotsParPage.value = nbLotsRecus.value;
                nbPages.value = recalculerNbPages();

                genererListePagination();
                chercherLotsAAfficher();
            } else {
                console.error("Réponse invalide:", reponse);
            }
        } catch (error) {
            console.error("Erreur lors du chargement des lots:", error);
        } finally {
            chargement.value = false;
        }
    });

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
        lotsAffiches.value = [];

        let positionDebut = (pageCourante.value - 1) * lotsParPage.value;
        let positionFin = pageCourante.value * lotsParPage.value;

        for (
            let i = positionDebut;
            i < positionFin && i < listeLots.value.length;
            i++
        ) {
            lotsAffiches.value.push(listeLots.value[i]);
        }
    }

    const chargerLots = async () => {
        try {
            const reponse = await store.dispatch("chercherTousLotsParEncan", props.idEncan);
            if (reponse?.data) {
                store.commit('setLots', reponse.data);
                listeLots.value = reponse.data;
                nbLotsRecus.value = reponse.data.length;

                // Charger les mises de l'utilisateur si connecté
                if (store.state.isLoggedIn) {
                    await store.dispatch("fetchUserBids");
                }
            }
        } catch (error) {
            console.error("Erreur lors du chargement des lots:", error);
        }
        chargement.value = false;
    };

    // Appeler chargerLots au montage du composant
    onMounted(() => {
        chargerLots();
    });

    // Ajouter ce computed
    const isLoggedIn = computed(() => store.state.isLoggedIn);

    // Ajouter ce watch pour recharger les lots quand l'utilisateur se connecte
    watch(isLoggedIn, async (newValue) => {
        if (newValue) {
            await store.dispatch("fetchUserBids");
            await chargerLots();
        }
    });
</script>

<style scoped></style>
