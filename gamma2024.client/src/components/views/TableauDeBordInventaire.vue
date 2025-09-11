<template>
    <div class="px-3">
        <div class="container">

            <h1 class="text-center mb-2 mb-md-5">Liste des lots</h1>

            <div v-if="messageConfirmation" class="alert alert-success py-0">
                {{ messageConfirmation }}
            </div>

            <h3 class="text-center" for="Recherche">Rechercher un lot</h3>

            <div class="d-flex gap-3 align-items-center mb-5">
                <div class="dropdown">
                    <button class="btn btnSurvolerBleuMoyenFond boutonPersonnalise text-white dropdown-toggle"
                            type="button"
                            id="dropdownMenuButton"
                            data-toggle="dropdown"
                            data-bs-toggle="dropdown"
                            aria-haspopup="true"
                            aria-expanded="false">
                        Recherche avancée
                    </button>
                    <ul class="dropdown-menu">
                        <li class="d-flex dropdown-item">
                            <input class="checkboxTousRecherche d-flex-1"
                                   type="checkbox"
                                   id="tousSelectionnerrCheckboxRecherche"
                                   checked />
                            <label class="d-flex-1" for="tousSelectionnerrCheckboxRecherche">
                                Tous Sélectionner
                            </label>
                        </li>
                        <li v-for="(visible, colonne, index) in colonnesVisibles"
                            :key="colonne"
                            class="d-flex justify-content-start dropdown-item">
                            <div v-if="index <=12">
                                <input class="checkboxSeulRecherche d-flex-1"
                                       type="checkbox"
                                       :id="`lot${ colonne.charAt(0).toUpperCase() + colonne.slice(1) } CheckboxRecherche`"
                                       checked
                                       disabled />
                                <label class="d-flex-1"
                                       :for="`lot${colonne.charAt(0).toUpperCase() + colonne.slice(1)}CheckboxRecherche`">
                                    {{(colonne.charAt(0).toUpperCase() + colonne.slice(1)).replace(/([A-Z])/g," $1")}}
                                </label>
                            </div>
                        </li>
                    </ul>
                </div>

                <input data-bs-theme="light"
                       type="search"
                       aria-label="Recherche"
                       v-model="rechercheDansListeDeLot"
                       placeholder="Rechercher un lot"
                       class="form-control insertionRecherche" />
            </div>

            <div>
                <button class="btn fs-5 btn-block w-100 btnSurvolerBleuMoyenFond btnClick text-white"
                        type="button"
                        id="ajouterLotButton"
                        @click="redirigerVersCreationLot">
                    Ajouter un lot
                </button>
            </div>
        </div>

        <div class="d-flex gap-2 justify-content-center mt-4" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des lots...</span>
            </div>
            <p>Chargement des lots en cours...</p>
        </div>

        <div v-if="!chargement" class="w-100">
            <div class="d-flex flex-column flex-md-row justify-content-center justify-content-md-between my-4">
                <div class="dropdown d-flex justify-content-center mb-2 mb-md-0" v-if="lotsAffiches.length">
                    <button class="btn btnSurvolerBleuMoyenFond boutonPersonnalise text-white dropdown-toggle"
                            type="button"
                            id="dropdownMenuButton"
                            data-toggle="dropdown"
                            data-bs-toggle="dropdown"
                            aria-haspopup="true"
                            aria-expanded="false">
                        Sélectionner les colonnes
                    </button>
                    <ul class="dropdown-menu">
                        <li class="d-flex justify-content-start dropdown-item">
                            <input class="checkboxTous d-flex-1"
                                   type="checkbox"
                                   id="tousSelectionnerrCheckbox"
                                   v-model="tousSelectionner"
                                   @change="toggleToutesColonnes" />
                            <label class="d-flex-1"
                                   for="tousSelectionnerrCheckbox">
                                Tous Sélectionner
                            </label>
                        </li>
                        <li v-for="(visible, colonne) in colonnesVisibles" :key="colonne" class="d-flex justify-content-start dropdown-item">
                            <input class="checkboxSeul d-flex-1"
                                   type="checkbox"
                                   :id="`lot${colonne.charAt(0).toUpperCase() + colonne.slice(1)}Checkbox`"
                                   :checked="visible"
                                   :disabled="tousSelectionner"
                                   @change="toggleColonne(colonne)" />
                            <label class="d-flex-1" :for="`lot${colonne.charAt(0).toUpperCase() + colonne.slice(1)}Checkbox`">
                                {{ (colonne.charAt(0).toUpperCase() + colonne.slice(1)).replace(/([A-Z])/g," $1") }}
                            </label>
                        </li>
                    </ul>
                </div>

                <div class="d-flex justify-content-center" v-if="lotsAffiches.length">
                    <div class="d-flex flex-row gap-2">
                        <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                                type="button"
                                @click="afficherLotsParPage(20)"
                                v-bind:disabled="lotsParPage == 20">
                            20
                        </button>
                        <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                                type="button"
                                @click="afficherLotsParPage(50)"
                                v-bind:disabled="lotsParPage == 50">
                            50
                        </button>
                        <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                                type="button"
                                @click="afficherLotsParPage(100)"
                                v-bind:disabled="lotsParPage == 100">
                            100
                        </button>
                        <button class="d-flex align-items-center text-center rounded btn text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond"
                                type="button"
                                @click="afficherTousLots"
                                v-bind:disabled="lotsParPage == nbLotsRecus">
                            Tous
                        </button>
                    </div>
                </div>
            </div>

            <div class="d-flex justify-content-center" v-if="!lotsAffiches.length">
                <h2>Aucun résultat trouvé</h2>
            </div>

            <div class="overflow-auto" v-else>
                <table class="table table-striped text-center">
                    <colgroup id="colgroup">
                        <col id="colEncan">
                        <col id="colNumero">
                        <col id="colPrixOuverture">
                        <col id="colValeurMinPourVente">
                        <col id="colEstimationMin">
                        <col id="colEstimationMax">
                        <col id="colCategorie">
                        <col id="colArtiste">
                        <col id="colDimensions">
                        <col id="colDescription">
                        <col id="colMedium">
                        <col id="colVendeur">
                        <col id="colVendu">
                        <col id="colLivraison">
                        <col id="colModifier">
                        <col id="colSupprimer">
                    </colgroup>
                    <thead class="table-dark">
                        <tr class="align-middle">
                            <th v-if="colonnesVisibles.encan">Encan</th>
                            <th v-if="colonnesVisibles.numero">Lot #</th>
                            <th v-if="colonnesVisibles.prixOuverture">Prix Ouverture</th>
                            <th v-if="colonnesVisibles.valeurMinPourVente">Valeur Min Pour Vente</th>
                            <th v-if="colonnesVisibles.estimationMin">Estimation Min</th>
                            <th v-if="colonnesVisibles.estimationMax">Estimation Max</th>
                            <th v-if="colonnesVisibles.categorie">Catégorie</th>
                            <th v-if="colonnesVisibles.artiste">Artiste</th>
                            <th v-if="colonnesVisibles.dimension">Dimension (en po)</th>
                            <th v-if="colonnesVisibles.description">Description</th>
                            <th v-if="colonnesVisibles.medium">Medium</th>
                            <th v-if="colonnesVisibles.vendeur">Vendeur</th>
                            <th v-if="colonnesVisibles.estVendu">Vendu</th>
                            <th v-if="colonnesVisibles.livraison">Livraison</th>
                            <th v-if="colonnesVisibles.modifier"></th>
                            <th v-if="colonnesVisibles.supprimer"></th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="lot in lotsAffiches" :key="lot.id" class="align-middle">
                            <td v-if="colonnesVisibles.encan">{{ lot.numeroEncan }}</td>
                            <td v-if="colonnesVisibles.numero">{{ lot.code }}</td>
                            <td v-if="colonnesVisibles.prixOuverture">{{ lot.prixOuverture }} $</td>
                            <td v-if="colonnesVisibles.valeurMinPourVente">
                                <span v-if="lot.prixMinPourVente==0"></span>
                                <span v-else>{{ lot.prixMinPourVente }} $</span>
                            </td>
                            <td v-if="colonnesVisibles.estimationMin">{{ lot.valeurEstimeMin }} $</td>
                            <td v-if="colonnesVisibles.estimationMax">{{ lot.valeurEstimeMax }} $</td>
                            <td v-if="colonnesVisibles.categorie">{{ lot.categorie }}</td>
                            <td v-if="colonnesVisibles.artiste">{{ lot.artiste }}</td>
                            <td v-if="colonnesVisibles.dimension">{{ lot.hauteur }} x {{ lot.largeur }}</td>
                            <td v-if="colonnesVisibles.description">{{ lot.description }}</td>
                            <td v-if="colonnesVisibles.medium">{{ lot.medium }}</td>
                            <td v-if="colonnesVisibles.vendeur">{{ lot.vendeur }}</td>
                            <td v-if="colonnesVisibles.estVendu">
                                <span v-if="lot.estVendu">{{ lot.mise }}$</span>
                                <img v-else src="/icons/NonVendu.png" width="40" height="40" />
                            </td>
                            <td v-if="colonnesVisibles.livraison">
                                <img v-if="lot.estLivrable" src="/icons/Livrable.png" width="40" height="40" />
                                <img v-else src="/icons/NonLivrable.png" width="40" height="40" />
                            </td>
                            <td>
                                <div class="d-flex" v-if="!lot.estVendu && parseInt(numeroEncanCourrant)!=lot.numeroEncan">
                                    <router-link :to="{ name: 'ModificationLot', params: { id: lot.id } }">
                                        <button class="btn btnModifierIcone bleuMarinSecondaireFond px-3 me-3">
                                            <img src="/icons/ModifierBtn.png" width="30" height="30" />
                                        </button>
                                    </router-link>

                                    <button class="btn btn-danger px-3 btn_delete">
                                        <img @click="ouvrirBoiteModale(lot.id)"
                                             src="/icons/SupprimerBtn.png"
                                             width="25"
                                             height="30" />
                                    </button>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="d-flex flex-row justify-content-center gap-1 flex-wrap p-3" v-if="lotsAffiches.length">
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
                            @click="changerPage(item)"
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

            <div v-if="lotASupprimer !== null" class="modal-overlay">
                <div class="modal-content">
                    <p>Êtes-vous sûr de vouloir supprimer ce lot ?</p>
                    <div class="modal-buttons">
                        <button @click="confirmerSuppression" class="btn btn-danger">OK</button>
                        <button @click="annulerSuppression" class="btn btn-secondary">
                            Annuler
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { onMounted, ref, watch, computed } from "vue";
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";

    const store = useStore();
    const router = useRouter();
    const lots = ref([]);
    const lotASupprimer = ref(null);
    const messageConfirmation = ref(null);

    const lotsFiltres = ref([]);
    const rechercheDansListeDeLot = ref("");
    const nbLotsRecus = ref();
    const lotsParPage = ref();
    const listePagination = ref([]);
    const pageCourante = ref(1);
    const lotsAffiches = ref();
    const chargement = ref(true);
    const nbPages = ref();
    const numeroEncanCourrant = ref()
    let genererListeDeLotsFiltree = function () { };

    const colonnesVisibles = ref({
        encan: true,
        numero: true,
        prixOuverture: true,
        valeurMinPourVente: true,
        estimationMin: true,
        estimationMax: true,
        categorie: true,
        artiste: true,
        dimension: true,
        description: true,
        medium: true,
        vendeur: true,
        estVendu: true,
        livraison: true,
    });

    const tousSelectionner = ref(true);

    const toggleToutesColonnes = () => {
        if (tousSelectionner.value) {
            Object.keys(colonnesVisibles.value).forEach((key) => {
                colonnesVisibles.value[key] = true;
            });
        }
    };

    const toggleColonne = (colonne) => {
        if (!tousSelectionner.value) {
            colonnesVisibles.value[colonne] = !colonnesVisibles.value[colonne];
        }
    };

    const toggleToutesColonnesRecherche = () => {
        const nouvelEtat = !Object.values(colonnesRecherche.value).every((v) => v);
        Object.keys(colonnesRecherche.value).forEach((key) => {
            colonnesRecherche.value[key] = nouvelEtat;
        });
    };

    const toggleColonneRecherche = (colonne) => {
        colonnesRecherche.value[colonne] = !colonnesRecherche.value[colonne];
    };

    const changerNbLotParPage = (nouvLotsParPage) => {
        lotsParPage.value = nouvLotsParPage;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        genererListePagination();
        chercherLotsAAfficher();
    };

    const afficherTousLots = () => {
        lotsParPage.value = nbLotsRecus.value;
        nbPages.value = recalculerNbPages();
        pageCourante.value = 1;
        genererListePagination();
        chercherLotsAAfficher();
    };

    const reculerPage = ref(function (evenement) {
        if (pageCourante.value > 1) {
            pageCourante.value--;
            chercherLotsAAfficher(evenement);
        }
    });

    const avancerPage = ref(function (evenement) {
        if (pageCourante.value < nbPages.value) {
            pageCourante.value++;
            chercherLotsAAfficher(evenement);
        }
    });

    const changerPage = ref(function (numeroPage) {
        if (numeroPage !== "...") {
            pageCourante.value = parseInt(numeroPage);
            chercherLotsAAfficher();
        }
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

    function chercherLotsAAfficher(evenement) {
        lotsAffiches.value = [];
        let positionDebut = (pageCourante.value - 1) * lotsParPage.value;
        let positionFin = pageCourante.value * lotsParPage.value;
        for (
            let i = positionDebut;
            i < positionFin && i < lotsFiltres.value.length;
            i++
        ) {
            lotsAffiches.value.push(lotsFiltres.value[i]);
            lotsAffiches.value.sort(function (x, y) {
                var xNb = parseInt(x.code.replace(/[^0-9]+/g, ""));
                var yNb = parseInt(y.code.replace(/[^0-9]+/g, ""));

                if (xNb == yNb) {
                    var xLtr = x.code.replace(/[^A-Za-z]+/g, "");
                    var yLtr = y.code.replace(/[^A-Za-z]+/g, "");

                    return xLtr.localeCompare(yLtr)
                }
                else if (xNb < yNb) {
                    return -1;
                }
                else {
                    return 1;
                }
            })
            lotsAffiches.value.sort((a, b) => a.numeroEncan.localeCompare(b.numeroEncan));
        }
    }

    onMounted(async () => {
        const checkboxQuiSelectionneToutRecherche = document.querySelector(
            ".checkboxTousRecherche"
        );
        const listeDesCheckboxesRecherche = document.querySelectorAll(
            ".checkboxSeulRecherche"
        );

        try {
            initialiserDonnees();

        } catch (erreur) {
            console.error("Erreur lors de la récupération des lots:", erreur);
        }

        listeDesCheckboxesRecherche.forEach((el, index) => {
            el.addEventListener("click", function (e) {
                var valeur = rechercheDansListeDeLot.value.toLowerCase();
                rechercherAvance(valeur);
            });
        });

        checkboxQuiSelectionneToutRecherche.addEventListener("change", function (e) {
            if (this.checked) {
                listeDesCheckboxesRecherche.forEach((el, index) => {
                    el.checked = true;
                    el.disabled = true;
                });
            } else {
                listeDesCheckboxesRecherche.forEach((el) => {
                    el.disabled = false;
                    el.checked = false;
                });
            }
            var valeur = rechercheDansListeDeLot.value.toLowerCase();
            rechercherAvance(valeur);
        });

        genererListeDeLotsFiltree = function () {
            var lotsAFiltres = lots.value;
            lotsFiltres.value = [];

            let rechercheEnMinuscule = rechercheDansListeDeLot.value.toLowerCase();
            lotsFiltres.value = lotsAFiltres.filter((lot) => {
                if (
                    listeDesCheckboxesRecherche[0].checked &&
                    lot.numeroEncan
                        .toString()
                        .toLowerCase()
                        .startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[1].checked &&
                    lot.code.toString().toLowerCase().startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[2].checked &&
                    lot.prixOuverture
                        .toString()
                        .toLowerCase()
                        .startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[3].checked &&
                    lot.prixMinPourVente
                        .toString()
                        .toLowerCase()
                        .startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[4].checked &&
                    lot.valeurEstimeMin
                        .toString()
                        .toLowerCase()
                        .startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[5].checked &&
                    lot.valeurEstimeMax
                        .toString()
                        .toLowerCase()
                        .startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[6].checked &&
                    lot.categorie.toString().toLowerCase().startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[7].checked &&
                    lot.artiste.toString().toLowerCase().startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[8].checked &&
                    lot.hauteur.toString().toLowerCase().startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[8].checked &&
                    lot.largeur.toString().toLowerCase().startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[9].checked &&
                    lot.description
                        .toString()
                        .toLowerCase()
                        .startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[10].checked &&
                    lot.medium.toString().toLowerCase().startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[11].checked &&
                    lot.vendeur.toString().toLowerCase().startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } else if (
                    listeDesCheckboxesRecherche[12].checked &&
                    lot.mise
                        .toString()
                        .toLowerCase()
                        .startsWith(rechercheEnMinuscule)
                ) {
                    return true;
                } return false;
            });
        };

        let rechercherAvance = function (nouvelleValeur) {
            if (nouvelleValeur == "" || nouvelleValeur == null) {
                lotsFiltres.value = lots.value;
            } else {
                genererListeDeLotsFiltree();
            }
            nbLotsRecus.value = lotsFiltres.value.length;
            pageCourante.value = 1;
            nbPages.value = recalculerNbPages();
            genererListePagination();
            chercherLotsAAfficher();
        };

        watch(rechercheDansListeDeLot, (nouvelleValeur) => {
            rechercherAvance(nouvelleValeur);
        });
    });

    async function initialiserDonnees() {
        const reponse = await store.dispatch("obtenirTousLots");
        lots.value = reponse.map((lot) => ({
            ...lot,
        }));
        lotsFiltres.value = lots.value;
        nbLotsRecus.value = lotsFiltres.value.length;
        lotsParPage.value = nbLotsRecus.value;
        nbPages.value = recalculerNbPages();

        genererListePagination();

        chercherLotsAAfficher();
        chargement.value = false;

        const numEncCour = await store.dispatch("chercherEncanEnCours");
        numeroEncanCourrant.value = numEncCour.data.numeroEncan;
    }

    watch(rechercheDansListeDeLot, (nouvelleValeur) => {
        rechercherAvance(nouvelleValeur);
    });

    function rechercherAvance(valeur) {
        if (!valeur) {
            lotsFiltres.value = lots.value;
        } else {
            valeur = valeur.toLowerCase();
            lotsFiltres.value = lots.value.filter((lot) => {
                return Object.entries(lot).some(([key, value]) => {
                    if (typeof value === "string" || typeof value === "number") {
                        return value.toString().toLowerCase().includes(valeur);
                    }
                    return false;
                });
            });
        }
        nbLotsRecus.value = lotsFiltres.value.length;
        pageCourante.value = 1;
        nbPages.value = recalculerNbPages();
        genererListePagination();
        chercherLotsAAfficher();
    }

    const ouvrirBoiteModale = (id) => {
        lotASupprimer.value = id;
    };

    const confirmerSuppression = async () => {
        if (lotASupprimer.value !== null) {
            try {
                await store.dispatch("supprimerLot", lotASupprimer.value);
                lots.value = lots.value.filter((lot) => lot.id !== lotASupprimer.value);
                messageConfirmation.value = "Lot supprimé avec succès.";
                setTimeout(() => {
                    messageConfirmation.value = null;
                }, 2000);
            } catch (error) {
                console.error("Erreur lors de la suppression du lot:", error);
            } finally {
                lotASupprimer.value = null;
            }
        }
        await initialiserDonnees();
    };

    const annulerSuppression = () => {
        lotASupprimer.value = null;
    };

    const redirigerVersCreationLot = () => {
        router.push({ name: "CreationLot" });
    };

    const afficherLotsParPage = (nbLots) => {
        changerNbLotParPage(nbLots);
    };
</script>

<style scoped>
    .boutonPersonnalise {
        padding-left: 15px;
        padding-right: 15px;
        border: none;
        border-radius: 5px;
    }

    li label {
        font-size: 13px;
    }

    .checkboxTousRecherche,
    .checkboxSeulRecherche,
    .checkboxTous,
    .checkboxSeul {
        margin-right: 7px;
    }

    .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: rgba(0, 0, 0, 0.5);
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .modal-content {
        background-color: white;
        padding: 20px;
        border-radius: 5px;
        text-align: center;
    }

    .modal-buttons {
        margin-top: 15px;
    }

        .modal-buttons button {
            margin: 0 10px;
        }

    .alert {
        margin-left: 10px 0;
        margin-right: 10px 0;
        padding-left: 10px;
        padding-right: 10px;
        border-radius: 5px;
        height: 30px;
    }

    .alert-success {
        background-color: #d4edda;
        color: #155724;
    }

    .btn_delete {
        background-color: rgb(194, 8, 8);
    }

        .btn_delete:hover {
            background-color: rgb(235, 6, 6);
        }

    table {
        box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.2), 0 3px 5px 0 rgba(0, 0, 0, 0.19);
    }

    th {
        font-size: 14px;
    }

    td {
        font-size: 14px;
    }

    .insertionRecherche {
        box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.2), 0 3px 5px 0 rgba(0, 0, 0, 0.19);
    }
</style>
