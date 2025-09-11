<template lang="">
    <div class="container">
        <h1 class="text-center md-2 mb-md-5">Liste des membres</h1>

        <h3 class="text-center">Rechercher un membre</h3>

        <input v-model="searchQuery"
               class="form-control row col-10 ms-1"
               type="search"
               placeholder="Rechercher un membre"
               aria-label="Search" />

        <div class="d-flex gap-2 justify-content-center mt-4" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des membres...</span>
            </div>
            <p>Chargement des membres en cours...</p>
        </div>

        <div v-if="!chargement" class="w-100">
            <div class="d-flex justify-content-center justify-content-md-end my-4" v-if="membresAffiches.length">
                <div class="d-flex flex-row gap-2">
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbMembreParPage(20)"
                            v-bind:disabled="membresParPage == 20">
                        20
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbMembreParPage(50)"
                            v-bind:disabled="membresParPage == 50">
                        50
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            @click="changerNbMembreParPage(100)"
                            v-bind:disabled="membresParPage == 100">
                        100
                    </button>
                    <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                            type="button"
                            @click="afficherTousMembres"
                            v-bind:disabled="membresParPage == nbMembresRecus">
                        Tous
                    </button>
                </div>
            </div>

            <div class="d-flex justify-content-center mt-4" v-if="!membresAffiches.length">
                <h2>Aucun résultat trouvé</h2>
            </div>

            <div v-else class="overflow-auto">
                <table class="table table-striped col-md-12 text-center">
                    <thead class="table-dark">
                        <tr>
                            <th scope="col">Nom</th>
                            <th scope="col">Prénom</th>
                            <th scope="col">Pseudonyme</th>
                            <th scope="col">Courriel</th>
                            <th scope="col">Voir</th>
                            <th scope="col">État</th>
                        </tr>
                    </thead>
                    <tbody class="fs-6">
                        <tr v-for="membre in membresAffiches" :key="membre.id">
                            <td class="align-middle">{{ membre.name }}</td>
                            <td class="align-middle">{{ membre.firstName }}</td>
                            <td class="align-middle">{{ membre.userName }}</td>
                            <td class="align-middle">{{ membre.email }}</td>
                            <td class="align-middle">
                                <span>
                                    <button class="btn btn-info" @click="detailsDuMembre(membre.id)">
                                        <img src="/icons/VoirBtn.png" height="40" width="40" alt="..." />
                                    </button>
                                </span>
                            </td>
                            <td class="align-middle">
                                <span v-if="membre.estBloque">
                                    <button class="btn btn-danger" @click="debloquerUnMembre(membre)">
                                        <img src="/icons/CadenasFerme.png" height="40" width="40" alt="..." />
                                    </button>
                                </span>
                                <span v-else>
                                    <button class="btn btn-success" @click="bloquerUnMembre(membre)">
                                        <img src="/icons/CadenasOuvert.png" height="40" width="40" alt="..." />
                                    </button>
                                </span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="d-flex flex-row justify-content-center gap-1 flex-wrap p-3" v-if="membresAffiches.length != 0">
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
</template>

<script setup>
    import { ref, onMounted, computed, watch } from "vue";
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";

    const store = useStore();
    const router = useRouter();
    let mesMembres = ref([]);
    const searchQuery = ref("");

    const chargement = ref(true);

    const nbMembresRecus = ref();
    const membresParPage = ref();
    const listePagination = ref([]);
    const pageCourante = ref(1);
    const nbPages = ref();
    const membresAffiches = ref([]);

    onMounted(async () => {
        try {
            mettreAJourDonnees();
        } catch (error) {
            console.error("Erreur lors de la récupération des membres:", error);
        }
    });

    function detailsDuMembre(idMembre) {
        router.push({ name: "detailsMembre", params: { id: idMembre } });
    }

    const bloquerUnMembre = async (membre) => {
        await store.dispatch("bloquerUnMembre", membre.id);
        mettreAJourDonnees();
    };

    const debloquerUnMembre = async (membre) => {
        await store.dispatch("debloquerUnMembre", membre.id);
        mettreAJourDonnees();
    };

    const mettreAJourDonnees = async function () {
        const reponse = await store.dispatch("ObtenirTousLesMembres");
        mesMembres.value = reponse.map((membre) => ({
            ...membre,
        }));

        nbMembresRecus.value = membresFiltres.value.length;
        membresParPage.value = nbMembresRecus.value;
        nbPages.value = recalculerNbPages();

        genererListePagination();
        chercherMembresAAfficher();

        chargement.value = false;
    };

    // Propriété calculée pour filtrer les membres en fonction de la recherche
    const membresFiltres = computed(() => {
        return tousLesMembres.value.filter((membre) => {
            const rechercheEnMinuscule = searchQuery.value.toLowerCase();
            return (
                membre.name.toLowerCase().includes(rechercheEnMinuscule) ||
                membre.firstName.toLowerCase().includes(rechercheEnMinuscule) ||
                membre.userName.toLowerCase().includes(rechercheEnMinuscule) ||
                membre.email.toLowerCase().includes(rechercheEnMinuscule)
            );
        });
    });

    const tousLesMembres = computed(() => {
        if (mesMembres.value) {
            return mesMembres.value;
        }
        return [];
    });

    watch(membresFiltres, () => {
        membresAffiches.value = membresFiltres.value;

        nbMembresRecus.value = membresFiltres.value.length;
        pageCourante.value = 1;
        AjusterPagination();
    });

    const changerNbMembreParPage = ref(function (nouvMembresParPage) {
        membresParPage.value = nouvMembresParPage;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        AjusterPagination();
    });

    const afficherTousMembres = ref(function () {
        membresParPage.value = nbMembresRecus.value;
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
        return Math.ceil(nbMembresRecus.value / membresParPage.value);
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

    function chercherMembresAAfficher() {
        membresAffiches.value = [];

        let positionDebut = (pageCourante.value - 1) * membresParPage.value;
        let positionFin = pageCourante.value * membresParPage.value;

        for (
            let i = positionDebut;
            i < positionFin && i < membresFiltres.value.length;
            i++
        ) {
            membresAffiches.value.push(membresFiltres.value[i]);
        }
    }

    watch(pageCourante, () => {
        genererListePagination();
        chercherMembresAAfficher();
    });

    function AjusterPagination() {
        nbPages.value = recalculerNbPages();
        genererListePagination();
        chercherMembresAAfficher();
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
</style>
