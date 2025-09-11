<template>
    <div class="d-flex flex-column align-items-center pb-3">
        <h1>Résultat de la recherche d'encans</h1>
        <div class="d-flex gap-2 w-100 justify-content-center" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des encans...</span>
            </div>
            <p>Chargement des encans en cours...</p>
        </div>

        <div v-else class="w-100">
            <h5 class="text-center" v-if="nbEncansRecus == 0">Aucun encan trouvé</h5>
            <div v-else>
                <div class="d-flex flex-row-reverse w-100 px-4 me-2 gap-2">
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            type="button"
                            @click="afficherTousEncans"
                            v-bind:disabled="encansParPage == nbEncansRecus">
                        Tous
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbEncanParPage(100)"
                            v-bind:disabled="encansParPage == 100">
                        100
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbEncanParPage(50)"
                            v-bind:disabled="encansParPage == 50">
                        50
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbEncanParPage(20)"
                            v-bind:disabled="encansParPage == 20">
                        20
                    </button>
                </div>
                <div class="d-flex gap-2" v-if="chargement">
                    <div class="spinner-border" role="status">
                        <span class="visually-hidden">Chargement des encans...</span>
                    </div>
                    <p>Chargement des encans en cours...</p>
                </div>
                <div v-else class="d-flex justify-content-center w-100">
                    <h5 class="text-center" v-if="encans.length < 1">
                        Votre demande de recherhce ne correspond à aucun encan.
                    </h5>

                    <div v-else class="w-100 px-3 row row-cols-lg-2 row-cols-1">
                        <div v-for="index in encansAffiche"
                             :key="index.numeroEncan"
                             class="col py-3">
                            <span @click="voirEncan(index.numeroEncan)">
                                <AffichageEncanTuile :encan="index" />
                            </span>
                        </div>
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
    </div>
</template>

<script setup>
    import AffichageEncanTuile from "@/components/views/AffichageEncanTuile.vue";
    import { computed, ref, watch, onMounted } from "vue";
    import { useRouter, useRoute } from "vue-router";
    import { useStore } from "vuex";
    import moment from "moment";

    const store = useStore();
    const router = useRouter();
    const route = useRoute();

    const listeEncans = ref([]);
    const encans = ref([]);
    var encansFiltres = ref([]);
    const nbEncansRecus = ref();
    const encansParPage = ref();
    const listePagination = ref([]);
    const pageCourante = ref(1);
    const encansAffiche = ref();
    const nbPages = ref();
    const chargement = ref(true);
    const numEncanCours = ref(0);

    onMounted(async () => {
        try {
            initialiseDonnees();
        } catch (erreur) { }
    });

    const voirEncan = ref(function (numeroEncanRecu) {
        if (numeroEncanRecu == numEncanCours.value) {
            router.push({ name: "EncanPresent" });
        } else {
            router.push({
                name: "Encan",
                params: { numeroEncan: numeroEncanRecu },
            });
        }
    });

    const queryChangement = computed(() => route.query);
    watch(queryChangement, initialiseDonnees);

    async function initialiseDonnees() {
        const reponse = await store.dispatch("chercherTousEncansVisibles");
        encans.value = reponse.data.map((encan) => ({
            ...encan,
        }));
        genererListeDEncansFiltree();
        nbEncansRecus.value = encansFiltres.value.length;
        encansParPage.value = nbEncansRecus.value;
        nbPages.value = recalculerNbPages();

        genererListePagination();

        chercherEncansAAfficher();
        chargement.value = false;
    }

    function genererListeDEncansFiltree() {
        encansFiltres.value = encans.value;
        filtrerLesEncansParNumeroEncan();
        filtrerLesEncansParDate();
    }

    function filtrerLesEncansParNumeroEncan() {
        var stringDeRequete = JSON.parse(route.query.data);
        encansFiltres.value = encansFiltres.value.filter((encan) => {
            if (stringDeRequete.stringNumeroEncan) {
                if (stringDeRequete.selectNumeroEncan == 0) {
                    if (encan.numeroEncan.toString() == stringDeRequete.stringNumeroEncan) {
                        return true;
                    }
                    return false;
                }
                if (
                    stringDeRequete.selectNumeroEncan == 1 &&
                    stringDeRequete.stringNumeroEncan2
                ) {
                    if (
                        encan.numeroEncan.toString() >= stringDeRequete.stringNumeroEncan &&
                        encan.numeroEncan.toString() <= stringDeRequete.stringNumeroEncan2
                    ) {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        });
    }

    function filtrerLesEncansParDate() {
        var stringDeRequete = JSON.parse(route.query.data);
        encansFiltres.value = encansFiltres.value.filter((encan) => {
            var encanDateDebut = moment(encan.dateDebut).format("yyyy-MM-DD");
            var encanDateFin = moment(encan.dateFin).format("yyyy-MM-DD");
            if (stringDeRequete.stringDate) {
                if (stringDeRequete.selectDate == 0) {
                    if (
                        moment(encanDateDebut).isSameOrBefore(stringDeRequete.stringDate) &&
                        moment(encanDateFin).isSameOrAfter(stringDeRequete.stringDate)
                    ) {
                        return true;
                    }
                    return false;
                }
                if (stringDeRequete.selectDate == 1) {
                    if (moment(encanDateDebut).isSameOrBefore(stringDeRequete.stringDate)) {
                        return true;
                    }
                    return false;
                }
                if (stringDeRequete.selectDate == 2) {
                    if (moment(encanDateFin).isSameOrAfter(stringDeRequete.stringDate)) {
                        return true;
                    }
                    return false;
                }
                if (stringDeRequete.selectDate == 3 && stringDeRequete.stringDate2) {
                    if (
                        moment(encanDateDebut).isSameOrBefore(stringDeRequete.stringDate2) &&
                        moment(encanDateFin).isSameOrAfter(stringDeRequete.stringDate)
                    ) {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        });
    }

    watch(pageCourante, () => {
        genererListePagination();
        chercherEncansAAfficher();
    });

    const changerNbEncanParPage = ref(function (nouvEncansParPage) {
        encansParPage.value = nouvEncansParPage;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        genererListePagination();
        chercherEncansAAfficher();
    });

    const afficherTousEncans = ref(function () {
        encansParPage.value = nbEncansRecus.value;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        genererListePagination();
        chercherEncansAAfficher();
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
            i < positionFin && i < encansFiltres.value.length;
            i++
        ) {
            encansAffiche.value.push(encansFiltres.value[i]);
        }
    }
</script>

<style scoped></style>
