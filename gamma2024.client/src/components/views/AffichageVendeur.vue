<template>
    <div class="container">
        <h1 class="text-center mb-2 mb-md-5">Liste des vendeurs</h1>

        <h3 class="text-center">Rechercher un vendeur</h3>

        <input v-model="searchQuery"
               class="form-control mb-5"
               type="search"
               placeholder="Rechercher un vendeur"
               aria-label="Search" />

        <router-link :to="{ name: 'VendeurCreation'}"
                     class="text-decoration-none">
            <button class="btn fs-5 btn-block w-100 btnSurvolerBleuMoyenFond btnClick text-white">
                Ajouter un vendeur
            </button>
        </router-link>

        <div class="d-flex gap-2 justify-content-center mt-4" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des vendeurs...</span>
            </div>
            <p>Chargement des vendeurs en cours...</p>
        </div>

        <div v-if="!chargement" class="w-100">
            <div class="d-flex justify-content-center justify-content-md-end my-4" v-if="vendeursAffiches.length">
                <div class="d-flex flex-row gap-2">
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbVendeurParPage(20)"
                            v-bind:disabled="vendeursParPage == 20">
                        20
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbVendeurParPage(50)"
                            v-bind:disabled="vendeursParPage == 50">
                        50
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbVendeurParPage(100)"
                            v-bind:disabled="vendeursParPage == 100">
                        100
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            type="button"
                            @click="afficherTousVendeurs"
                            v-bind:disabled="vendeursParPage == nbVendeursRecus">
                        Tous
                    </button>
                </div>
            </div>

            <div class="d-flex justify-content-center mt-4" v-if="!vendeursAffiches.length">
                <h2>Aucun résultat trouvé</h2>
            </div>

            <div v-else class="overflow-y-auto">
                <table class="table table-striped text-center">
                    <thead class="table-dark">
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">Nom</th>
                            <th scope="col">Prénom</th>
                            <th scope="col">Courriel</th>
                            <th scope="col">Téléphone</th>
                            <th scope="col">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(vendeur, index) in vendeursAffiches" :key="vendeur.id">
                            <td scope="row" class="align-middle">{{ index + 1 }}</td>
                            <td class="align-middle">{{ vendeur.nom }}</td>
                            <td class="align-middle">{{ vendeur.prenom }}</td>
                            <td class="align-middle">{{ vendeur.courriel }}</td>
                            <td class="align-middle">{{ vendeur.telephone }}</td>
                            <td>
                                <button class="btn btnModifierIcone bleuMarinSecondaireFond px-3">
                                    <router-link :to="{name: 'vendeurModification', params: { id: vendeur.id.toString() }}"
                                                 class="text-decoration-none">
                                        <img src="/icons/ModifierBtn.png"
                                             width="30"
                                             alt="Editer" />
                                    </router-link>
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="d-flex flex-row justify-content-center gap-1 flex-wrap p-3" v-if="vendeursAffiches.length">
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
    import { ref, onMounted, computed, watch } from "vue";
    import { useStore } from "vuex";

    const store = useStore();
    const vendeurs = ref(null);
    const searchQuery = ref("");

    const nbVendeursRecus = ref();
    const vendeursParPage = ref();
    const listePagination = ref([]);
    const pageCourante = ref(1);
    const nbPages = ref();
    const vendeursAffiches = ref([]);

    const chargement = ref(true);

    const vendeursAffichage = computed(() => {
        if (vendeurs.value) {
            return vendeurs.value;
        }
        return [];
    });

    onMounted(async () => {
        try {
            vendeurs.value = await store.dispatch("obtenirTousVendeurs");

            nbVendeursRecus.value = vendeursFiltres.value.length;
            vendeursParPage.value = nbVendeursRecus.value;
            nbPages.value = recalculerNbPages();

            genererListePagination();
            chercherVendeursAAfficher();

            chargement.value = false;
        } catch (error) {
            console.error("Erreur lors de la récupération des vendeurs:", error);
        }
    });

    const vendeursFiltres = computed(() => {
        return vendeursAffichage.value.filter((vendeur) => {
            const searchLower = searchQuery.value.toLowerCase();
            return (
                vendeur.nom.toLowerCase().includes(searchLower) ||
                vendeur.prenom.toLowerCase().includes(searchLower) ||
                vendeur.courriel.toLowerCase().includes(searchLower) ||
                vendeur.telephone.toLowerCase().includes(searchLower)
            );
        });
    });

    watch(vendeursFiltres, () => {
        vendeursAffiches.value = vendeursFiltres.value;

        nbVendeursRecus.value = vendeursFiltres.value.length;
        pageCourante.value = 1;
        AjusterPagination();
    });

    const changerNbVendeurParPage = ref(function (nouvVendeursParPage) {
        vendeursParPage.value = nouvVendeursParPage;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        AjusterPagination();
    });

    const afficherTousVendeurs = ref(function () {
        vendeursParPage.value = nbVendeursRecus.value;
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
        return Math.ceil(nbVendeursRecus.value / vendeursParPage.value);
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

    function chercherVendeursAAfficher() {
        vendeursAffiches.value = [];

        let positionDebut = (pageCourante.value - 1) * vendeursParPage.value;
        let positionFin = pageCourante.value * vendeursParPage.value;

        for (
            let i = positionDebut;
            i < positionFin && i < vendeursFiltres.value.length;
            i++
        ) {
            vendeursAffiches.value.push(vendeursFiltres.value[i]);
        }
    }

    watch(pageCourante, () => {
        genererListePagination();
        chercherVendeursAAfficher();
    });

    function AjusterPagination() {
        nbPages.value = recalculerNbPages();
        genererListePagination();
        chercherVendeursAAfficher();
    }
</script>

<style scoped>
    .btn {
        color: #ffffff;
    }

    .table {
        color: #333333;
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
