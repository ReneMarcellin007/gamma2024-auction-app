<template>
    <span v-if="idFactureLivraisonCreee != ''">
        <FactureLivraisonModal :idFacture="idFactureLivraisonCreee"></FactureLivraisonModal>
    </span>

    <div class="container d-flex flex-column align-items-center">
        <h1>Choix de livraison</h1>

        <p id="message" v-show="siMessage"></p>

        <div v-if="idFactureLivraisonCreee != ''" class="d-flex flex-column gap-2">
            <button class="btn btnSurvolerBleuMoyenFond text-white"
                    data-bs-toggle="modal"
                    :data-bs-target="'#modalFactureLivraison'+idFactureLivraisonCreee">
                Voir la facture de livraison
            </button>

            <router-link :to="{name:'HistoriqueAchatsParMembre'}" class="text-decoration-none">
                <button type="button" class="btn btnSurvolerBleuMoyenFond text-white">
                    Retour vers l'historique des achats
                </button>
            </router-link>
        </div>

        <div v-if="chargement">
            <div class="spinner-border" role="status"></div>
        </div>
        <div v-else-if="!siMessage" class="d-flex flex-column">
            <p>Vos achats durant l'encan {{facture.facture.numeroEncan}} sont livrables. Veuillez faire votre choix de livraison.</p>

            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th scope="col">Numéro du lot</th>
                        <th scope="col">Description</th>
                        <th scope="col">Prix vendu</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="fac in facture.facture.lots" :key="fac.id">
                        <td scope="row">{{ fac.numero }}</td>
                        <td>{{ fac.description }}</td>
                        <td>{{ fac.mise }}$</td>
                    </tr>
                </tbody>
            </table>

            <div class="form-check form-check-inline">
                <input class="form-check-input mt-7"
                       type="radio"
                       name="inlineRadioOptions"
                       id="aucuneLivraison"
                       value="aucune"
                       checked
                       @change="
            montrerFormLivraison = false;
            siDonation = false;
          " />
                <label class="form-check-label" for="aucuneLivraison">Cueillette</label>
            </div>
            <div class="form-check form-check-inline">
                <input class="form-check-input mt-7"
                       type="radio"
                       name="inlineRadioOptions"
                       id="livraison"
                       value="livraison"
                       @change="montrerFormLivraison = true" />
                <label class="form-check-label" for="livraison">Livraison</label>
            </div>

            <div v-if="montrerFormLivraison">
                <div>
                    <table class="table">
                        <tbody>
                            <tr>
                                <th scope="col" class="text-start">Frais de livraison</th>
                                <td scope="col" class="text-end">
                                    {{ facture.sousTotal.toFixed(2) }} $
                                </td>
                            </tr>
                            <tr>
                                <th scope="col" class="text-start">TPS</th>
                                <td scope="col" class="text-end">
                                    {{ facture.tps.toFixed(2) }} $
                                </td>
                            </tr>
                            <tr>
                                <th scope="col" class="text-start">TVQ</th>
                                <td scope="col" class="text-end">
                                    {{ facture.tvq.toFixed(2) }} $
                                </td>
                            </tr>
                            <tr v-if="siDonation">
                                <th scope="col" class="text-start">Don</th>
                                <td scope="col" class="text-end">
                                    {{ facture.don.toFixed(2) }} $
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <div class="form-check">
                        <input class="form-check-input mt-7"
                               type="checkbox"
                               value=""
                               id="donationCheck"
                               @change="toggleDonation" />
                        <label class="form-check-label" for="donationCheck">
                            Donation
                        </label>
                    </div>

                    <div v-if="siDonation">
                        <select class="form-select py-0"
                                aria-label="Charite select"
                                id="chariteSelList">
                            <option v-for="charite in facture.charites"
                                    class="py-0"
                                    :value="charite.id"
                                    selected>
                                {{ charite.nomOrganisme }}
                            </option>
                        </select>
                    </div>

                    <div class="mt-15">
                        <div class="d-flex flex-row justify-content-between">
                            <label class="form-check-label"> Adresse: </label>
                            <div class="form-check">
                                <input class="form-check-input mt-7"
                                       type="checkbox"
                                       value=""
                                       id="adresseCheck"
                                       @change="toggleAdresse" />
                                <label class="form-check-label"
                                       for="adresseCheck"
                                       id="adresseLabel">
                                    Utiliser une autre adresse
                                </label>
                            </div>
                        </div>

                        <select class="form-select py-0"
                                aria-label="Adresse select"
                                id="adresseSelList"
                                v-if="!siAutreAdresse">
                            <option v-for="adresse in adresses"
                                    class="py-0"
                                    :value="adresse.id"
                                    selected>
                                {{ adresse.appartement }} {{ adresse.numeroCivique }}
                                {{ adresse.rue }}, {{ adresse.ville }}, {{ adresse.province }},
                                {{ adresse.pays }}, {{ adresse.codePostal }}
                            </option>
                        </select>

                        <form v-show="siAutreAdresse">
                            <div class="alert alert-warning"
                                 role="alert"
                                 v-show="siAlerte"
                                 id="messageAdresse">
                                A simple warning alert—check it out!
                            </div>
                            <div class="form-group">
                                <label>Numéro civique</label>
                                <input type="number"
                                       class="form-control"
                                       id="numCiv"
                                       placeholder="1"
                                       v-model="adresse.numeroCivique" />
                            </div>

                            <div class="form-group">
                                <label>Appartement/Bureau</label>
                                <input type="text"
                                       class="form-control"
                                       id="apt"
                                       placeholder="Apt 12"
                                       v-model="adresse.appartement" />
                            </div>

                            <div class="form-group">
                                <label>Rue</label>
                                <input type="text"
                                       class="form-control"
                                       id="rue"
                                       placeholder="Rue Saint-Jaques"
                                       v-model="adresse.rue" />
                            </div>

                            <div class="form-group">
                                <label>Ville</label>
                                <input type="text"
                                       class="form-control"
                                       id="ville"
                                       placeholder="Granby"
                                       v-model="adresse.ville" />
                            </div>

                            <div class="form-group">
                                <label for="province">Province </label>
                                <select id="province"
                                        placeholder="Sélectionnez une province"
                                        class="form-select">
                                    <option v-for="(province, index) in provinces"
                                            :key="province.value"
                                            :value="province.value"
                                            :selected="index == 8">
                                        {{ province.text }}
                                    </option>
                                </select>
                            </div>

                            <div class="form-group">
                                <label>Pays</label>
                                <input type="text"
                                       class="form-control"
                                       id="pays"
                                       value="Canada"
                                       readonly />
                            </div>

                            <div class="form-group">
                                <label>Code Postal</label>
                                <InputMask type="text"
                                           id="codePostal"
                                           mask="a9a 9a9"
                                           placeholder="A1A 2B2"
                                           class="form-control"
                                           v-model="adresse.codePostal" />
                            </div>
                        </form>
                    </div>

                    <div class="mt-15">
                        <label class="form-check-label"> Carte: </label>
                        <select class="form-select py-0"
                                aria-label="Default select example"
                                id="carteSelList">
                            <option v-for="carte in cartes"
                                    class="py-0"
                                    :value="carte.paymentMethodId"
                                    selected>
                                {{ carte.marque.toUpperCase() }} -
                                {{ carte.dernier4Numero }} ({{ carte.expirationDate }})
                            </option>
                        </select>
                    </div>

                    <div class="mt-15">
                        <h2 v-if="siDonation" class="ps-0 mb-0">
                            TOTAL: {{ facture.prixFinal.toFixed(2) }} $
                        </h2>
                        <h2 v-else class="ps-0 mb-0">
                            TOTAL: {{ facture.prixFinalSansDon.toFixed(2) }} $
                        </h2>
                    </div>
                </div>
            </div>

            <div class="mt-7">
                <button id="submit"
                        class="btn btn-outline text-white btnSurvolerBleuMoyenFond btnDesactiverBleuMoyenFond my-2 col-3"
                        @click="enregistrerChoixLivraison">
                    <div class="spinner-grow"
                         id="spinner"
                         v-if="chargementSauvegarde"></div>
                    <span id="button-text" v-else>Enregistrer</span>
                </button>
            </div>
        </div>
    </div>
</template>

<script setup>
    import InputMask from "primevue/inputmask";
    import Button from "primevue/button";
    import { onMounted, ref } from "vue";
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";
    import FactureLivraisonModal from "@/components/modals/VoirFactureLivraisonModal.vue";

    const router = useRouter();

    const store = useStore();
    const props = defineProps({
        idFacture: String,
    });
    const facture = ref({});
    const chargement = ref(true);
    const montrerFormLivraison = ref(false);
    const siDonation = ref(false);
    const siAutreAdresse = ref(false);
    const toggleDonation = ref(function () {
        siDonation.value = document.getElementById("donationCheck").checked;
    });
    const toggleAdresse = ref(function () {
        siAutreAdresse.value = document.getElementById("adresseCheck").checked;
        if (siAutreAdresse.value) {
            document
                .getElementById("codePostal")
                .classList.remove("p-inputtext", "p-component", "p-inputmask");
        }
    });
    const adresses = ref([]);
    const siMessage = ref(false);
    const siAlerte = ref(false);
    const chargementSauvegarde = ref(false);
    const cartes = ref([]);
    const adresse = ref({
        numeroCivique: "",
        rue: "",
        appartement: "",
        ville: "",
        province: "",
        pays: "Canada",
        codePostal: "",
    });

    const provinces = ref([
        { text: "Alberta", value: "AB" },
        { text: "Colombie-Britannique", value: "BC" },
        { text: "Manitoba", value: "MB" },
        { text: "Nouveau-Brunswick", value: "NB" },
        { text: "Terre-Neuve-et-Labrador", value: "NL" },
        { text: "Nouvelle-Écosse", value: "NS" },
        { text: "Ontario", value: "ON" },
        { text: "Île-du-Prince-Édouard", value: "PE" },
        { text: "Québec", value: "QC" },
        { text: "Saskatchewan", value: "SK" },
        { text: "Territoires du Nord-Ouest", value: "NT" },
        { text: "Nunavut", value: "NU" },
        { text: "Yukon", value: "YT" },
    ]);
    const idFactureLivraisonCreee = ref("");

    onMounted(async () => {
        try {
            const reponse = await store.dispatch(
                "chercherPrevisualisationLivraison",
                parseInt(props.idFacture)
            );
            facture.value = reponse.data;

            const adresseReponse = await store.dispatch("chercherAdressesClient");
            adresses.value = adresseReponse.data;

            const cartesReponse = await store.dispatch("chercherCartesUser");
            cartes.value = cartesReponse.data;
        } catch (error) {
            siMessage.value = true;
            document.getElementById("message").innerText = error.reponse.data;
        }
        chargement.value = false;
    });

    const enregistrerChoixLivraison = ref(async function () {
        chargementSauvegarde.value = true;
        document.querySelector("#submit").disabled = true;
        try {
            siAlerte.value = false;
            var choixLivraison = {};
            var valide = true;
            if (montrerFormLivraison.value) {
                var chariteSelList = document.getElementById("chariteSelList");
                var adresseSelList = document.getElementById("adresseSelList");
                var carteSelList = document.getElementById("carteSelList");
                var provinceSelList = document.getElementById("province");
                var idChariteSel = null;
                var divMessage = document.getElementById("messageAdresse");

                if (chariteSelList != undefined) {
                    idChariteSel = chariteSelList.value;
                }

                if (siAutreAdresse.value) {
                    adresse.value.province = provinceSelList.value;

                    if (adresse.value.codePostal == "") {
                        divMessage.innerHTML = "Code postal invalide";
                        valide = false;
                        siAlerte.value = true;
                    } else if (adresse.value.numeroCivique == "") {
                        divMessage.innerHTML = "Numéro civique invalide";
                        valide = false;
                        siAlerte.value = true;
                    } else if (adresse.value.rue == "") {
                        divMessage.innerHTML = "Rue invalide";
                        valide = false;
                        siAlerte.value = true;
                    } else if (adresse.value.ville == "") {
                        divMessage.innerHTML = "Ville invalide";
                        valide = false;
                        siAlerte.value = true;
                    }

                    adresse.value.numeroCivique = adresse.value.numeroCivique.toString();
                    choixLivraison = {
                        don: siDonation.value,
                        idCharite: idChariteSel,
                        idFacture: props.idFacture,
                        idAdresse: 0,
                        adresse: JSON.parse(JSON.stringify(adresse.value)),
                        pmId: carteSelList.value,
                    };
                } else {
                    choixLivraison = {
                        don: siDonation.value,
                        idCharite: idChariteSel,
                        idFacture: props.idFacture,
                        idAdresse: adresseSelList.value,
                        adresse: null,
                        pmId: carteSelList.value,
                    };
                }
            } else {
                choixLivraison = {
                    don: null,
                    idCharite: null,
                    idFacture: props.idFacture,
                    idAdresse: null,
                    pmId: null,
                    adresse: null
                };
            }
            if (valide) {

                const reponse = await store.dispatch("enregistrerChoixLivraison", choixLivraison)
                idFactureLivraisonCreee.value = reponse.data;

                siMessage.value = true
                document.getElementById("message").innerText = "Choix de livraison sauvegardé et payé."

                if (idFactureLivraisonCreee.value == '0') {
                    siMessage.value = true
                    document.getElementById("message").innerText = "Choix de livraison sauvegardé."

                    setTimeout(() => {
                        router.push({ name: 'HistoriqueAchatsParMembre' })
                    }, 5000);
                }
            }
            else {
                chargementSauvegarde.value = false;
                document.querySelector("#submit").disabled = false;
            }
        } catch (error) {
            siMessage.value = true
            document.getElementById("message").innerText = error.reponse
        }
    });
</script>
<style>
    .mt-7 {
        margin-top: 7px;
    }

    .mt-15 {
        margin-top: 15px;
    }
</style>
