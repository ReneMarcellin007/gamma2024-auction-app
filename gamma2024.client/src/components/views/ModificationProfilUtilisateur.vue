<template>
    <div v-if="enChargement">Chargement...</div>
    <div class="pt-md-5 container">
        <div class="container d-flex flex-column justify-content-start align-items-stretch card p-md-5 mb-5">
            <div class="card-header mt-3 mx-3">
                <!-- Ajout de la section avatar -->
                <div class="avatar-section mb-4 d-flex flex-column align-items-center">
                    <div class="avatar-container">
                        <img :src="avatarUrl"
                             alt="Avatar"
                             class="avatar-image mb-2"
                             @click="declencherInsertionFichier"
                             @error="gererImageErreur" />
                    </div>
                    <button @click="declencherInsertionFichier" class="btn btnSurvolerBleuMoyenFond text-white mt-3">
                        Modifier l'avatar
                    </button>
                    <input type="file"
                           ref="insertionFichier"
                           @change="gererChangerFichier"
                           accept="image/*"
                           style="display: none" />
                </div>

                <h2 class="fs-1 text-center fw-bold mt-5">Profil d'utilisateur</h2>
            </div>
            <div v-if="message"
                 :class="[
          'alert',
          message.type === 'success' ? 'alert-success' : 'alert-danger',
        ]">
                {{ message.text }}
            </div>

            <div class="text-center mt-1">

                <router-link :to="{name: 'GestionCartes'}">
                    <h5>Gestion des cartes de credit</h5>
                </router-link>

            </div>

            <div class="mt-md-3 card-body">
                <form @submit.prevent="soumettreFormulaire">
                    <!-- Informations générales -->
                    <div class="card mb-5">
                        <div class="card-header">Informations générales</div>
                        <div class="card-body">
                            <div class="row mt-md-3">
                                <div class="col-md-6">
                                    <label for="nom">Nom</label>
                                    <input type="text"
                                           id="nom"
                                           :class="[
                      'form-control',
                      { 'is-invalid': vuelidate.generalInfo.nom.$error },
                    ]"
                                           v-model="formData.generalInfo.nom"
                                           @blur="vuelidate.generalInfo.nom.$touch()" />
                                    <div class="invalid-feedback" v-if="vuelidate.generalInfo.nom.$error">
                                        {{ vuelidate.generalInfo.nom.$errors[0].$message }}
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label for="prenom">Prénom</label>
                                    <input type="text"
                                           id="prenom"
                                           :class="[
                      'form-control',
                      { 'is-invalid': vuelidate.generalInfo.prenom.$error },
                    ]"
                                           v-model="formData.generalInfo.prenom"
                                           @blur="vuelidate.generalInfo.prenom.$touch()" />
                                    <div class="invalid-feedback"
                                         v-if="vuelidate.generalInfo.prenom.$error">
                                        {{ vuelidate.generalInfo.prenom.$errors[0].$message }}
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-md-3">
                                <div class="col-md-6">
                                    <label for="pseudonyme">Pseudonyme</label>
                                    <input type="text"
                                           id="pseudonyme"
                                           :class="[
                      'form-control',
                      { 'is-invalid': vuelidate.generalInfo.pseudo.$error },
                    ]"
                                           v-model="formData.generalInfo.pseudo"
                                           @blur="vuelidate.generalInfo.pseudo.$touch()" />
                                </div>
                                <div class="col-md-6">
                                    <label for="telephone">Téléphone</label>
                                    <div class="input-wrapper">
                                        <InputMask type="tel"
                                                   mask="(999) 999-9999"
                                                   :class="[
                        'form-control',
                        { 'is-invalid': vuelidate.generalInfo.telephone.$error },
                      ]"
                                                   id="telephone"
                                                   v-model="formData.generalInfo.telephone"
                                                   @blur="vuelidate.generalInfo.telephone.$touch()" />
                                        <div class="invalid-feedback"
                                             v-if="vuelidate.generalInfo.telephone.$error">
                                            {{ vuelidate.generalInfo.telephone.$errors[0].$message }}
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-md-3">
                                <div class="col-md-6">
                                    <label class="labels">Courriel</label>
                                    <input type="text"
                                           class="form-control"
                                           v-model="formData.generalInfo.courriel" />
                                </div>
                                <div class="d-flex flex-column col-md-6">
                                    <label class="labels">Mot de passe</label>
                                    <button type="button"
                                            class="btn border border-1 btn_change_password"
                                            @click="changerLaVisibilite">
                                        <img src="/icons/CadenasMotDePasse.png"
                                             class="img-fluid img_password"
                                             alt="icon_password" />
                                        <span class="ms-2">Changer le mot de passe</span>
                                    </button>
                                </div>
                            </div>
                            <div class="d-flex flex-column col-md-12 mt-md-3"
                                 v-if="changeVisible">
                                <div class="form-group">
                                    <label for="currentPassword">Mot de passe actuel</label>
                                    <input type="password"
                                           v-model="formData.generalInfo.currentPassword"
                                           id="currentPassword"
                                           class="form-control"
                                           @input="validerMotDePasseInseres" />
                                </div>
                                <div class="form-group"
                                     v-if="formData.generalInfo.currentPassword">
                                    <label for="newPassword">Nouveau mot de passe</label>
                                    <input type="password"
                                           v-model="formData.generalInfo.newPassword"
                                           id="newPassword"
                                           class="form-control"
                                           :class="{ 'is-invalid': vuelidate.generalInfo.newPassword.$error }"
                                           @input="validerMotDePasseInseres" />
                                    <div class="invalid-feedback"
                                         v-if="vuelidate.generalInfo.newPassword.$error">
                                        Le nouveau mot de passe est requis et doit avoir au moins 8
                                        caractères.
                                    </div>
                                </div>
                                <div class="form-group"
                                     v-if="formData.generalInfo.currentPassword">
                                    <label for="confirmNewPassword">Confirmer le nouveau mot de passe</label>
                                    <input type="password"
                                           v-model="formData.generalInfo.confirmNewPassword"
                                           id="confirmNewPassword"
                                           class="form-control"
                                           :class="{
                      'is-invalid': vuelidate.generalInfo.confirmNewPassword.$error,
                    }"
                                           @input="validerMotDePasseInseres" />
                                    <div class="invalid-feedback"
                                         v-if="vuelidate.generalInfo.confirmNewPassword.$error">
                                        La confirmation du mot de passe est requise et doit
                                        correspondre au nouveau mot de passe.
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Informations d'adresse -->
                    <div class="card my-5">
                        <div class="card-header">Adresse</div>
                        <div class="card-body">
                            <div class="row mt-md-3">
                                <div class="col-md-6">
                                    <label for="numeroCivique">Numéro civique</label>
                                    <input type="text"
                                           v-model="formData.adresse.numeroCivique"
                                           :class="[
                      'form-control',
                      { 'is-invalid': vuelidate.adresse.numeroCivique.$error },
                    ]"
                                           id="numeroCivique"
                                           @blur="vuelidate.adresse.numeroCivique.$touch()" />
                                    <div class="invalid-feedback"
                                         v-if="vuelidate.adresse.numeroCivique.$error">
                                        {{ vuelidate.adresse.numeroCivique.$errors[0].$message }}
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label for="rue">Rue</label>
                                    <input type="text"
                                           v-model="formData.adresse.rue"
                                           :class="[
                      'form-control',
                      { 'is-invalid': vuelidate.adresse.rue.$error },
                    ]"
                                           id="rue"
                                           @blur="vuelidate.adresse.rue.$touch()" />
                                    <div class="invalid-feedback" v-if="vuelidate.adresse.rue.$error">
                                        {{ vuelidate.adresse.rue.$errors[0].$message }}
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-md-3">
                                <div class="col-md-6">
                                    <label for="appartement">Appartement/bureau</label>
                                    <input type="text"
                                           class="form-control"
                                           id="appartement"
                                           v-model="formData.adresse.appartement" />
                                </div>
                                <div class="col-md-6">
                                    <label for="ville">Ville</label>
                                    <input type="text"
                                           v-model="formData.adresse.ville"
                                           :class="[
                      'form-control',
                      { 'is-invalid': vuelidate.adresse.ville.$error },
                    ]"
                                           id="ville"
                                           @blur="vuelidate.adresse.ville.$touch()" />
                                    <div class="invalid-feedback" v-if="vuelidate.adresse.ville.$error">
                                        {{ vuelidate.adresse.ville.$errors[0].$message }}
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-md-3">
                                <div class="col-md-6">
                                    <label for="province">Province</label>
                                    <select id="province"
                                            name="province"
                                            v-model="formData.adresse.province"
                                            :class="[
                      'form-control',
                      { 'is-invalid': vuelidate.adresse.province.$error },
                    ]"
                                            @blur="vuelidate.adresse.province.$touch()">
                                        <option value="" disabled selected>
                                            Sélectionner une province
                                        </option>
                                        <option v-for="province in provinces"
                                                :key="province.value"
                                                :value="province.value">
                                            {{ province.text }}
                                        </option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <label class="labels">Pays</label>
                                    <input type="text"
                                           class="form-control"
                                           v-model="formData.adresse.pays" />
                                </div>
                            </div>
                            <div class="row mt-md-3">
                                <div class="col-md-12">
                                    <label class="labels">Code postal</label>
                                    <div class="input-wrapper">
                                        <inputMask type="text"
                                                   id="postalCode"
                                                   :class="[
                        'form-control',
                        { 'is-invalid': vuelidate.adresse.codePostal.$error },
                      ]"
                                                   mask="a9a 9a9"
                                                   v-model="formData.adresse.codePostal"
                                                   @blur="vuelidate.adresse.codePostal.$touch()" />
                                    </div>
                                    <div class="invalid-feedback"
                                         v-if="vuelidate.adresse.codePostal.$error">
                                        {{ vuelidate.adresse.codePostal.$errors[0].$message }}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="d-flex justify-content-start justify-content-md-end gap-2 py-3 card-footer flex-column flex-md-row px-3">
                        <button class="btn btn-secondary rounded-pill px-4"
                                :disabled="!formulaireChange"
                                type="button"
                                @click="effacerFormulaire">
                            Réinitialiser
                        </button>
                        <button type="submit"
                                :class="[
                formulaireEstValide && formulaireChange ? 'bleuValide' : 'bleuNonValide',
                'btn rounded-pill px-4',
              ]"
                                :disabled="!formulaireEstValide || !formulaireChange">
                            <div class="d-flex flex-row align-items-center justify-content-center">
                                <img src="/icons/EnregistrerBtn.png"
                                     class="img-fluid img_password"
                                     alt="icon_save" />
                                <span class="ms-2">Enregistrer</span>
                            </div>
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, reactive, computed, onMounted, watch } from "vue";
    import { useStore } from "vuex";
    import { useVuelidate } from "@vuelidate/core";
    import InputMask from "primevue/inputmask";
    import { required, email, helpers } from "@vuelidate/validators";
    import api from "@/services/api";
    import { useRouter } from "vue-router";
    import { toast } from "vue3-toastify";
    import ToastContent from "../Toast/toastConfirm.vue";
    import { h } from "vue";

    const store = useStore();
    const message = ref(null);
    const insertionFichier = ref(null);
    const messageRequis = helpers.withMessage("Ce champ est requis", required);
    const messageCourriel = helpers.withMessage(
        "Veuillez entrer un courriel valide",
        email
    );
    const router = useRouter();
    const clientInfo = computed(() => store.state.clientInfo);

    const apiBaseUrl = computed(() => {
        if (store.state.api && store.state.api.defaults) {
            return store.state.api.defaults.baseURL.replace("/api", "");
        }
        return "";
    });

    const avatarUrl = computed(() => {
        if (!store.getters.avatarUrl) return "/icons/Avatar.png";
        if (store.getters.avatarUrl.startsWith("http")) {
            return store.getters.avatarUrl;
        }
        // Le getter avatarUrl du store devrait déjà inclure le chemin complet
        return store.getters.avatarUrl;
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

    let changeVisible = ref(false);
    let changerLaVisibilite = () => {
        changeVisible.value = !changeVisible.value;
    };

    let formulaireChange = ref(false);
    // objet qui contient tous les champs remplis correctement
    let formData = reactive({
        generalInfo: {
            nom: "",
            prenom: "",
            courriel: "",
            telephone: "",
            pseudo: "",
            currentPassword: "",
            newPassword: "",
            confirmNewPassword: "",
        },
        adresse: {
            numeroCivique: "",
            rue: "",
            appartement: "",
            ville: "",
            province: "",
            pays: "Canada",
            codePostal: "",
        },
    });

    let formulaireInitial = ref({});

    let regles = computed(() => {
        return {
            generalInfo: {
                nom: { required: messageRequis },
                prenom: { required: messageRequis },
                courriel: { required: messageRequis, email: messageCourriel },
                telephone: { required: messageRequis },
                pseudo: { required: messageRequis },
                currentPassword: {
                    required: helpers.withMessage(
                        "Le mot de passe actuel est requis si vous souhaitez le changer",
                        (value) => !formData.generalInfo.newPassword || !!value
                    ),
                },
                newPassword: {
                    minLength: helpers.withMessage(
                        "Le mot de passe doit contenir au moins 8 caractères",
                        (value) => !value || value.length >= 8
                    ),
                },
                confirmNewPassword: {
                    sameAsPassword: helpers.withMessage(
                        "Les mots de passe ne correspondent pas",
                        (value) =>
                            !formData.generalInfo.newPassword ||
                            value === formData.generalInfo.newPassword
                    ),
                },
            },
            adresse: {
                numeroCivique: { required: messageRequis },
                rue: { required: messageRequis },
                appartement: {},
                ville: { required: messageRequis },
                province: { required: messageRequis },
                pays: { required: messageRequis },
                codePostal: { required: messageRequis },
            },
        };
    });

    const mapProvince = (province) => {
        const listeDeProvinces = {
            Québec: "QC",
            Ontario: "ON",
            Alberta: "AB",
            "Colombie-Britannique": "BC",
            Manitoba: "MB",
            "Nouveau-Brunswick": "NB",
            "Terre-Neuve-et-Labrador": "NL",
            "Nouvelle-Écosse": "NS",
            "Île-du-Prince-Édouard": "PE",
            Saskatchewan: "SK",
            "Territoires du Nord-Ouest": "NT",
            Nunavut: "NU",
            Yukon: "YT",
        };
        return listeDeProvinces[province] || province;
    };

    const vuelidate = useVuelidate(regles, formData);

    const messageErreur = ref("");
    const enChargement = ref(true);

    // Fonction pour mettre à jour formData avec les données du client
    const mettreAJourFormulaire = () => {
        if (clientInfo.value) {
            // Informations générales
            formData.generalInfo.nom = clientInfo.value.name || "";
            formData.generalInfo.prenom = clientInfo.value.firstName || "";
            formData.generalInfo.courriel = clientInfo.value.email || "";
            formData.generalInfo.telephone = clientInfo.value.phoneNumber || "";
            formData.generalInfo.pseudo = clientInfo.value.pseudonym || "";
            formData.generalInfo.currentPassword = ""; // Généralement laissé vide pour des raisons de sécurité
            formData.generalInfo.newPassword = ""; // Généralement laissé vide pour des raisons de sécurité
            formData.generalInfo.confirmNewPassword = ""; // Généralement laissé vide pour des raisons de sécurité

            // Adresse
            formData.adresse.numeroCivique = clientInfo.value.civicNumber || "";
            formData.adresse.rue = clientInfo.value.street || "";
            formData.adresse.appartement = clientInfo.value.apartment || "";
            formData.adresse.ville = clientInfo.value.city || "";
            if (mapProvince(clientInfo.value.province)) {
                formData.adresse.province = mapProvince(clientInfo.value.province) || "";
            }
            formData.adresse.pays = clientInfo.value.country || "Canada";
            formData.adresse.codePostal = clientInfo.value.postalCode || "";

            // Mise à jour de l'URL de l'avatar
            if (clientInfo.value.photo) {
                avatarUrl.value = `${store.state.api.defaults.baseURL.replace(
                    "/api",
                    ""
                )}${clientInfo.value.photo}`;
            } else {
                avatarUrl.value = "/icons/Avatar.png";
            }

            // Mise à jour de l'état initial du formulaire pour la comparaison ultérieure
            formulaireInitial.value = JSON.parse(JSON.stringify(formData));

            // Réinitialisation de l'indicateur de changement du formulaire
            formulaireChange.value = false;
        }
    };

    // Assurez-vous d'appeler cette fonction lorsque les données du client sont chargées
    // Par exemple, dans le hook onMounted ou dans un watcher sur clientInfo
    onMounted(async () => {
        try {
            await store.dispatch("fetchClientInfo");
            enChargement.value = false;
        } catch (erreur) {
            console.error("Erreur lors du chargement des informations client:", erreur);
            enChargement.value = false;
        }
    });

    // Ou utilisez un watcher pour mettre à jour le formulaire lorsque clientInfo change
    watch(clientInfo, mettreAJourFormulaire, { immediate: true });

    watch(
        () => JSON.parse(JSON.stringify(formData)), // Deep clone copy to avoid same ref type.
        () => {
            // Comparaison entre l'état initial et l'état actuel
            if (JSON.stringify(formData) !== JSON.stringify(formulaireInitial.value)) {
                formulaireChange.value = true;
            } else {
                formulaireChange.value = false;
            }
        },
        { deep: true }
    );

    const declencherInsertionFichier = () => {
        insertionFichier.value.click();
    };

    const gererImageErreur = () => {
        setTimeout(() => {
            if (!avatarUrl.value.includes("Avatar.png")) {
                avatarUrl.value = "/icons/Avatar.png";
            }
        }, 1000);
    };

    const gererChangerFichier = async (evenement) => {
        const file = evenement.target.files[0];
        if (file) {
            try {
                const fData = new FormData();
                fData.append("avatar", file);
                const reponse = await store.dispatch("updateAvatar", fData);

                // Utiliser l'URL complète retournée par l'action du store
                avatarUrl.value = reponse.data.avatarUrl;

                formulaireChange.value = true;
            } catch (erreur) {
                messageErreur.value =
                    "Erreur lors de la mise à jour de l'avatar: " +
                    (erreur.reponse?.data?.message || erreur.message);
            }
        }
    };

    const formulaireEstValide = computed(() => {
        const passwordFieldsEmpty =
            !formData.generalInfo.currentPassword &&
            !formData.generalInfo.newPassword &&
            !formData.generalInfo.confirmNewPassword;

        const passwordFieldsFilled =
            formData.generalInfo.currentPassword &&
            formData.generalInfo.newPassword &&
            formData.generalInfo.confirmNewPassword;

        const passwordFieldsValid =
            passwordFieldsEmpty ||
            (passwordFieldsFilled &&
                !vuelidate.value.generalInfo.currentPassword.$invalid &&
                !vuelidate.value.generalInfo.newPassword.$invalid &&
                !vuelidate.value.generalInfo.confirmNewPassword.$invalid);

        const otherFieldsValid =
            !vuelidate.value.generalInfo.nom.$invalid &&
            !vuelidate.value.generalInfo.prenom.$invalid &&
            !vuelidate.value.generalInfo.courriel.$invalid &&
            !vuelidate.value.generalInfo.telephone.$invalid &&
            !vuelidate.value.generalInfo.pseudo.$invalid &&
            !vuelidate.value.adresse.$invalid;

        return otherFieldsValid && passwordFieldsValid && formulaireChange.value;
    });

    // Ajoutez cette fonction pour gérer la validation des champs de mot de passe
    const validerMotDePasseInseres = () => {
        if (formData.generalInfo.currentPassword) {
            vuelidate.value.generalInfo.newPassword.$touch();
            vuelidate.value.generalInfo.confirmNewPassword.$touch();
        } else {
            vuelidate.value.generalInfo.newPassword.$reset();
            vuelidate.value.generalInfo.confirmNewPassword.$reset();
        }
    };

    watch(
        () => formData.generalInfo.currentPassword,
        (nouvelleValeur) => {
            if (nouvelleValeur) {
                validerMotDePasseInseres();
            }
        }
    );

    watch(
        () => [
            formData.generalInfo.currentPassword,
            formData.generalInfo.newPassword,
            formData.generalInfo.confirmNewPassword,
        ],
        () => {
            validerMotDePasseInseres();
        }
    );

    watch(
        () => store.state.user?.photo,
        (newPhotoUrl) => {
            if (newPhotoUrl) {
                avatarUrl.value = newPhotoUrl;
                formulaireChange.value = false;
            }
        },
        { deep: true }
    );

    const soumettreFormulaire = async () => {
        const resultat = await vuelidate.value.$validate();
        if (resultat) {
            try {
                const donneesMap = {
                    Name: formData.generalInfo.nom,
                    FirstName: formData.generalInfo.prenom,
                    Email: formData.generalInfo.courriel,
                    PhoneNumber: formData.generalInfo.telephone,
                    Pseudonym: formData.generalInfo.pseudo,
                    CurrentPassword: formData.generalInfo.currentPassword || undefined,
                    NewPassword: formData.generalInfo.newPassword || undefined,
                    ConfirmNewPassword:
                        formData.generalInfo.confirmNewPassword || undefined,
                    CivicNumber: formData.adresse.numeroCivique,
                    Street: formData.adresse.rue,
                    Apartment: formData.adresse.appartement,
                    City: formData.adresse.ville,
                    Province: formData.adresse.province,
                    Country: formData.adresse.pays,
                    PostalCode: formData.adresse.codePostal,
                    Photo: avatarUrl.value,
                };

                // Ne pas envoyer les champs de mot de passe s'ils sont tous vides
                if (
                    !donneesMap.CurrentPassword &&
                    !donneesMap.NewPassword &&
                    !donneesMap.ConfirmNewPassword
                ) {
                    delete donneesMap.CurrentPassword;
                    delete donneesMap.NewPassword;
                    delete donneesMap.ConfirmNewPassword;
                }

                await store.dispatch("updateClientInfo", donneesMap);
                toast.success(
                    h(ToastContent, {
                        title: "Informations mises à jour avec succès !",
                        description: "",
                    }),
                    {
                        position: toast.POSITION.TOP_CENTER,
                        autoClose: 3000,
                        pauseOnFocusLoss: false,
                        theme: "dark",
                        toastStyle: {
                            backgroundColor: "#052445",
                            color: "#fff",
                        },
                    }
                );

                // Réinitialiser les champs de mot de passe
                formData.generalInfo.currentPassword = "";
                formData.generalInfo.newPassword = "";
                formData.generalInfo.confirmNewPassword = "";

                await store.dispatch("fetchClientInfo");

                setTimeout(() => {
                    router.push("/");
                }, 2000);
            } catch (erreur) {
                message.value = {
                    type: "error",
                    text:
                        "Erreur lors de la mise à jour des informations: " +
                        (erreur.reponse?.data?.message || erreur.message),
                };
            }
        } else {
            message.value = {
                type: "error",
                text: "Formulaire non valide. Veuillez vérifier les champs.",
            };
        }
    };

    const effacerFormulaire = () => {
        Object.assign(formData, JSON.parse(JSON.stringify(formulaireInitial.value)));
        vuelidate.value.$reset();
        formulaireChange.value = false;
    };
</script>

<style scoped>
    .is-invalid {
        border-color: red;
        font-weight: 600;
    }

    .cadreBlanc {
        background-color: white;
        opacity: 0.4;
        border-radius: 15px;
        height: auto;
    }

    .bleuValide {
        background-color: #052445;
        color: white;
    }

    .bleuNonValide {
        background-color: #5a708a;
        color: white;
    }

    .avatar-section {
        text-align: center;
        margin-bottom: 20px;
        position: relative;
        z-index: 10;
    }

    .avatar-container {
        width: 100px;
        height: 100px;
        overflow: hidden;
        border-radius: 50%;
        margin: 0 auto 10px;
    }

    .avatar-image {
        width: 100%;
        height: 100%;
        object-fit: cover;
        cursor: pointer;
    }

    label {
        font-weight: 600;
        margin-left: 15px;
    }

    .input-wrapper .p-inputmask {
        width: 100%;
        border-radius: 0.35rem;
        border: 1px solid rgba(0, 0, 0, 0.12);
        padding: 0.375rem 0.75rem;
        background-color: white;
        height: calc(1.5em + 0.75rem + 2px);
        transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    }

    input::placeholder {
        color: #6c757d;
        opacity: 1;
    }

    .p-inputmask input::placeholder {
        color: #6c757d;
        opacity: 1;
        font-weight: 400;
    }

    .input-wrapper .p-inputmask.is-invalid {
        border-color: #dc3545;
    }

    .input-wrapper .p-inputmask:focus {
        border-color: #80bdff;
        outline: 0;
        box-shadow: 0 0 0 0.2rem #80bdff;
    }

    .input-wrapper .p-inputmask:hover {
        border: 1px solid rgba(0, 0, 0, 0.12);
    }

    .img_password {
        width: 15px;
        height: 17px;
    }

    .btn_change_password:hover {
        background-color: rgba(217, 217, 217, 0.273);
    }

    .card {
        border-radius: 18px;
        box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.2), 0 3px 5px 0 rgba(0, 0, 0, 0.19);
    }
</style>
