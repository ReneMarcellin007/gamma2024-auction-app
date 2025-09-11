<template>
    <div class="bg-image px-2 px-md-0 py-3 py-md-5 imageDeFondEsquise h-100">
        <div class="container d-flex flex-column justify-content-start align-items-stretch container col-md-6 bg-white bg-opacity-75 cadreBlanc px-3 px-md-5">
            <h2 class="fs-1 text-center fw-bold mt-5 ps-0">Inscription</h2>
            <p class="text-center">Obtenir un compte membre</p>

            <div v-if="messageErreur" class="alert alert-danger">
                {{ messageErreur }}
            </div>
            <div v-else>
                <div v-if="messageSucces" class="alert alert-success">
                    {{ messageSucces }}
                </div>
            </div>

            <!-- Stepper Navigation -->
            <div class="mt-3">
                <form @submit.prevent="soumettreFormulaire">
                    <Stepper v-model:activeIndex="activeIndex" :class="['mb-4']">
                        <!-- Informations générales -->
                        <StepItem :disabled="activeIndex !== 1" id="itemStep1">
                            <Step value="1" class="border-top border-3 p-2 border-dark fs-2">
                                <span>1 - Informations générales</span>
                            </Step>

                            <StepPanel v-if="activeIndex === 1" value="1">
                                <div class="container px-4 py-2">
                                    <div class="row mb-md-3 justify-content-around">
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="nom">Nom</label>
                                                <input type="text"
                                                       v-model="formData.generalInfo.nom"
                                                       :class="[
                            'form-control',
                            { 'is-invalid': vuelidate.generalInfo.nom.$error },
                          ]"
                                                       id="nom"
                                                       placeholder="Doe"
                                                       @blur="vuelidate.generalInfo.nom.$touch()" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.generalInfo.nom.$error">
                                                    {{ vuelidate.generalInfo.nom.$errors[0].$message }}
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="prenom">Prénom</label>
                                                <input type="text"
                                                       v-model="formData.generalInfo.prenom"
                                                       :class="[
                            'form-control',
                            { 'is-invalid': vuelidate.generalInfo.prenom.$error },
                          ]"
                                                       id="prenom"
                                                       placeholder="John"
                                                       @blur="vuelidate.generalInfo.prenom.$touch()" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.generalInfo.prenom.$error">
                                                    {{ vuelidate.generalInfo.prenom.$errors[0].$message }}
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mb-md-3 justify-content-around">
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="courriel">Courriel</label>
                                                <input type="email"
                                                       placeholder="johndoe@gmail.com"
                                                       v-model="formData.generalInfo.courriel"
                                                       :class="[
                            'form-control',
                            {
                              'is-invalid':
                                vuelidate.generalInfo.courriel.$error ||
                                (!emailDisponible && emailVerifie),
                              'is-valid': emailDisponible && emailVerifie,
                            },
                          ]"
                                                       id="courriel"
                                                       @blur="
                            vuelidate.generalInfo.courriel.$touch();
                            verifierEmail();
                          "
                                                       @input="emailVerifie = false" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.generalInfo.courriel.$error">
                                                    {{ vuelidate.generalInfo.courriel.$errors[0].$message }}
                                                </div>
                                                <div class="retroaction-invalide"
                                                     v-if="!emailDisponible && emailVerifie">
                                                    Cette adresse email est déjà utilisée.
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="telephone">Téléphone</label>
                                                <div class="input-wrapper">
                                                    <InputMask type="tel"
                                                               placeholder="(999) 999-9999"
                                                               mask="(999) 999-9999"
                                                               v-model="formData.generalInfo.telephone"
                                                               :class="[
                              'form-control',
                              { 'is-invalid': vuelidate.generalInfo.telephone.$error },
                            ]"
                                                               id="telephone"
                                                               @blur="vuelidate.generalInfo.telephone.$touch()" />
                                                    <div class="retroaction-invalide"
                                                         v-if="vuelidate.generalInfo.telephone.$error">
                                                        {{ vuelidate.generalInfo.telephone.$errors[0].$message }}
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mb-md-3 justify-content-around">
                                        <div class="col col-md-12">
                                            <div class="form-group">
                                                <label for="pseudonyme">Pseudonyme</label>
                                                <input type="text"
                                                       v-model="formData.generalInfo.pseudo"
                                                       :class="[
                            'form-control',
                            {
                              'is-invalid':
                                vuelidate.generalInfo.pseudo.$error ||
                                (!pseudoDisponible && pseudoVerifie),
                            },
                          ]"
                                                       id="pseudonyme"
                                                       placeholder="johnDoe45"
                                                       @blur="
                            vuelidate.generalInfo.pseudo.$touch();
                            verifierPseudo();
                          "
                                                       @input="pseudoVerifie = false" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.generalInfo.pseudo.$error">
                                                    {{ vuelidate.generalInfo.pseudo.$errors[0].$message }}
                                                </div>
                                                <div class="retroaction-valide"
                                                     v-if="!pseudoDisponible && pseudoVerifie">
                                                    Le pseudonyme est valide.
                                                </div>
                                                <div class="retroaction-valide"
                                                     v-if="pseudoDisponible && pseudoVerifie">
                                                    Ce pseudonyme est disponible.
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mb-md-3 justify-content-around">
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="motDePasse">Mot de passe</label>
                                                <input type="password"
                                                       v-model="formData.generalInfo.motDePasse"
                                                       :class="[
                            'form-control',
                            { 'is-invalid': vuelidate.generalInfo.motDePasse.$error },
                          ]"
                                                       id="motDePasse"
                                                       @blur="vuelidate.generalInfo.motDePasse.$touch()" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.generalInfo.motDePasse.$error">
                                                    {{ vuelidate.generalInfo.motDePasse.$errors[0].$message }}
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="confirmMotPasse">Confirmation de mot de passe</label>
                                                <input type="password"
                                                       v-model="formData.generalInfo.confirmMotPasse"
                                                       :class="[
                            'form-control',
                            {
                              'is-invalid':
                                vuelidate.generalInfo.confirmMotPasse.$error,
                            },
                          ]"
                                                       id="confirmMotPasse"
                                                       @blur="vuelidate.generalInfo.confirmMotPasse.$touch()" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.generalInfo.confirmMotPasse.$error">
                                                    {{
                            vuelidate.generalInfo.confirmMotPasse.$errors[0].$message
                                                    }}
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="d-flex justify-content-end gap-2 py-3">
                                    <button type="button"
                                            :class="[
                      state1 ? 'bleuValide' : 'bleuNonValide',
                      'btn rounded-pill px-5 me-4',
                    ]"
                                            :disabled="state1"
                                            @click="allerAuSuivantPrecedent(2)">
                                        Suivant
                                    </button>
                                </div>
                            </StepPanel>
                        </StepItem>
                        <StepItem :disabled="activeIndex !== 2" class="adresse">
                            <Step value="2" class="border-top border-3 p-2 border-dark fs-2">
                                <span>2 - Adresse</span>
                            </Step>

                            <StepPanel v-if="activeIndex === 2">
                                <div class="container px-4 py-2">
                                    <div class="row mb-md-3 justify-content-around">
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="numeroCivique">Numéro Civique</label>
                                                <input type="text"
                                                       v-model="formData.adresse.numeroCivique"
                                                       placeholder="1"
                                                       :class="[
                            'form-control',
                            { 'is-invalid': vuelidate.adresse.numeroCivique.$error },
                          ]"
                                                       id="numeroCivique"
                                                       @blur="vuelidate.adresse.numeroCivique.$touch()" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.adresse.numeroCivique.$error">
                                                    {{ vuelidate.adresse.numeroCivique.$errors[0].$message }}
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="rue">Rue</label>
                                                <input type="text"
                                                       v-model="formData.adresse.rue"
                                                       placeholder="Saint Jacques"
                                                       :class="[
                            'form-control',
                            { 'is-invalid': vuelidate.adresse.rue.$error },
                          ]"
                                                       id="rue"
                                                       @blur="vuelidate.adresse.rue.$touch()" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.adresse.rue.$error">
                                                    {{ vuelidate.adresse.rue.$errors[0].$message }}
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mb-md-3 justify-content-around">
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="appartement">Appartement/Bureau</label>
                                                <input type="text"
                                                       placeholder="12"
                                                       v-model="formData.adresse.appartement"
                                                       class="form-control"
                                                       id="appartement" />
                                            </div>
                                        </div>
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="ville">Ville</label>
                                                <input type="text"
                                                       placeholder="Granby"
                                                       v-model="formData.adresse.ville"
                                                       :class="[
                            'form-control',
                            { 'is-invalid': vuelidate.adresse.ville.$error },
                          ]"
                                                       id="ville"
                                                       @blur="vuelidate.adresse.ville.$touch()" />
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.adresse.ville.$error">
                                                    {{ vuelidate.adresse.ville.$errors[0].$message }}
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mb-md-3 justify-content-around">
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="province">Province </label>
                                                <select id="province"
                                                        name="province"
                                                        v-model="formData.adresse.province"
                                                        :class="[
                            'form-control',
                            { 'is-invalid': vuelidate.adresse.province.$error },
                          ]"
                                                        @blur="vuelidate.adresse.province.$touch()"
                                                        placeholder="Sélectionnez une province">
                                                    <option value="" disabled selected>
                                                        Sélectionner une province
                                                    </option>
                                                    <option v-for="province in provinces"
                                                            :key="province.value"
                                                            :value="province.value">
                                                        {{ province.text }}
                                                    </option>
                                                </select>
                                                <div class="retroaction-invalide"
                                                     v-if="vuelidate.adresse.province.$error">
                                                    {{ vuelidate.adresse.province.$errors[0].$message }}
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col col-md-6 col-12">
                                            <div class="form-group">
                                                <label for="pays">Pays</label>
                                                <input type="text"
                                                       class="form-control"
                                                       v-model="formData.adresse.pays"
                                                       id="pays" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="d-flex flex-row justify-content-between mb-md-3">
                                        <div class="col col-md-12">
                                            <div class="form-group">
                                                <label for="postalCode">Code Postal</label>
                                                <div class="input-wrapper">
                                                    <inputMask type="text"
                                                               v-model="formData.adresse.codePostal"
                                                               :class="[
                              'form-control',
                              { 'is-invalid': vuelidate.adresse.codePostal.$error },
                            ]"
                                                               id="postalCode"
                                                               mask="a9a 9a9"
                                                               placeholder="A1A 2B2"
                                                               @blur="vuelidate.adresse.codePostal.$touch()" />
                                                    <div class="retroaction-invalide"
                                                         v-if="vuelidate.adresse.codePostal.$error">
                                                        {{ vuelidate.adresse.codePostal.$errors[0].$message }}
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="d-flex justify-content-center justify-content-md-between gap-2 py-3 flex-md-row flex-column align-items-center">
                                    <button class="btn btn-secondary rounded-pill px-5 mb-3 mb-md-0"
                                            @click="allerAuSuivantPrecedent(1)">
                                        Précédent
                                    </button>

                                    <button type="submit"
                                            :disabled="stateFinal || isSubmitting"
                                            class="mb-md-0"
                                            :class="[
                      stateFinal ? 'bleuValide' : 'bleuNonValide',
                      classeActiveBouton,
                      'btn',
                    ]">
                                        <span v-if="isSubmitting"
                                              class="spinner-grow spinner-grow-sm"
                                              role="status"
                                              aria-hidden="true"></span>
                                        Créer le compte
                                    </button>
                                </div>
                            </StepPanel>
                        </StepItem>
                    </Stepper>
                </form>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, computed, reactive } from "vue";
    import Stepper from "primevue/stepper";
    import StepItem from "primevue/stepitem";
    import Step from "primevue/step";
    import InputMask from "primevue/inputmask";
    import StepPanel from "primevue/steppanel";
    import { h } from "vue";
    import { useVuelidate } from "@vuelidate/core";
    import ToastContent from "../Toast/toastConfirm.vue";
    import {
        required,
        email,
        helpers,
        sameAs,
        minLength,
        maxLength,
    } from "@vuelidate/validators";
    import { useStore } from "vuex";
    import { toast } from "vue3-toastify";
    import { useRouter } from "vue-router";

    const store = useStore();
    const router = useRouter();

    const messageErreur = ref("");
    const messageSucces = ref("");

    const activeIndex = ref(1);
    const messageRequis = helpers.withMessage("Ce champ est requis.", required);
    const messageCourriel = helpers.withMessage(
        "Veuillez entrer un courriel valide.",
        email
    );

    const messageLongueurMinimaleMotDePasse = helpers.withMessage(
        "Le mot de passe doit contenir au moins 8 caractères, minimum 1 majuscule, 1 minuscule, 1 chiffre et 1 caractère spécial.",
        minLength(8)
    );

    const messageLongueurMinimalePseudo = helpers.withMessage(
        "Le pseudo doit contenir au moins 3 caractères.",
        minLength(3)
    );

    const messageLongueurMaximalePseudo = helpers.withMessage(
        "Le pseudo doit contenir au maximum 20 caractères.",
        maxLength(20)
    );

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

    //objet qui contient tous les champs remplis correctement
    let formData = reactive({
        generalInfo: {
            nom: "",
            prenom: "",
            courriel: "",
            telephone: "",
            pseudo: "",
            motDePasse: "",
            confirmMotPasse: "",
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

    const messageIdentiqueAuMotDePasse = helpers.withMessage(
        "Les mots de passe ne correspondent pas.",
        sameAs(computed(() => formData.generalInfo.motDePasse))
    );

    const pseudoDisponible = ref(true);
    const pseudoVerifie = ref(false);
    const emailDisponible = ref(true);
    const emailVerifie = ref(false);

    //objet regles qui contient toutes les validations
    let regles = computed(() => {
        return {
            generalInfo: {
                nom: { required: messageRequis },
                prenom: { required: messageRequis },
                courriel: {
                    required: messageRequis,
                    email: messageCourriel,
                },
                telephone: { required: messageRequis },
                pseudo: {
                    required: messageRequis,
                    asyncValidator: async (value) => {
                        if (value) {
                            await verifierPseudo();
                            return pseudoDisponible.value;
                        }
                        return true;
                    },
                    minLength: messageLongueurMinimalePseudo,
                    maxLength: messageLongueurMaximalePseudo,
                },
                motDePasse: {
                    required: messageRequis,
                    minLength: messageLongueurMinimaleMotDePasse,
                },
                confirmMotPasse: {
                    required: messageRequis,
                    sameAsPassword: messageIdentiqueAuMotDePasse,
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

    const vuelidate = useVuelidate(regles, formData);
    const info = useVuelidate(regles.value.generalInfo, formData.generalInfo);
    const isSubmitting = ref(false);

    const classeActiveBouton = ref("btn rounded-pill px-5");

    const state1 = computed(() => {
        return info.value.$invalid;
    });

    const stateFinal = computed(() => {
        return vuelidate.value.$invalid;
    });

    function allerAuSuivantPrecedent(stepIndex) {
        activeIndex.value = stepIndex;
    }

    const verifierPseudo = async () => {
        if (formData.generalInfo.pseudo.length > 0) {
            try {
                pseudoVerifie.value = false;
                pseudoDisponible.value = await store.dispatch(
                    "verifierPseudonyme",
                    formData.generalInfo.pseudo
                );
                pseudoVerifie.value = true;
            } catch (error) {
                console.error("Erreur lors de la vérification du pseudonyme:", error);
                pseudoDisponible.value = false;
                pseudoVerifie.value = true;
            }
        }
    };

    const verifierEmail = async () => {
        if (
            formData.generalInfo.courriel.length > 0 &&
            !vuelidate.value.generalInfo.courriel.$error
        ) {
            try {
                emailVerifie.value = false;
                const emailEstDisponible = await store.dispatch(
                    "verifierEmail",
                    formData.generalInfo.courriel
                );
                emailDisponible.value = emailEstDisponible;
                emailVerifie.value = true;
            } catch (error) {
                console.error("Erreur lors de la vérification de l'email:", error);
                emailDisponible.value = false;
                emailVerifie.value = true;
            }
        }
    };

    const soumettreFormulaire = async () => {
        const result = await vuelidate.value.$validate();
        isSubmitting.value = true;

        if (!result) {
            isSubmitting.value = false;
            messageErreur.value =
                "Certaines informations du formulaire sont incorrectes";
            return;
        }

        try {
            const reponse = await store.dispatch("creerCompteUtilisateur", formData);
            if (reponse.success) {
                messageSucces.value = "Compte crée avec succès !";
                toast.success(
                    h(ToastContent, {
                        title: "Compte créé avec succès !",
                        description:
                            "Veuillez confirmer votre courriel afin d'activer votre compte",
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

                setTimeout(() => {
                    router.push("/connexion");
                }, 4000);
            } else {
                messageErreur.value =
                    reponse.message ||
                    "Une erreur est survenue lors de la création du compte.";
            }
        } catch (error) {
            messageErreur.value =
                "Une erreur est survenue lors de la création du compte.";
        } finally {
            isSubmitting.value = false;
        }
    };
</script>

<style scoped>
    label {
        font-weight: bolder;
        margin-left: 15px;
    }

    .is-invalid,
    span {
        border-color: red;
        font-weight: 600;
    }

    .cadreBlanc {
        border-radius: 50px;
        height: auto;
        width: auto;
    }

    .bleuNonValide {
        background-color: #052445;
        color: white;
    }

    .bleuValide {
        background-color: #5a708a;
        color: white;
    }

    .alert {
        margin-top: 1rem;
        padding: 0.75rem 1.25rem;
        border: 1px solid transparent;
        border-radius: 0.25rem;
    }

    .alert-danger {
        color: #721c24;
        background-color: #f8d7da;
        border-color: #f5c6cb;
    }

    .alert-success {
        color: #155724;
        background-color: #d4edda;
        border-color: #c3e6cb;
    }

    .retroaction-invalide {
        color: #dc3545;
        font-size: 0.875em;
        margin-top: 0.25rem;
        display: block;
    }

    .retroaction-valide {
        color: #28a745;
        font-size: 0.875em;
        margin-top: 0.25rem;
        display: block;
    }

    .is-invalid {
        border-color: #dc3545;
    }

    .is-valid {
        border-color: #28a745;
    }

    .input-wrapper .p-inputmask {
        width: 100%;
        border-radius: 0.25rem;
        padding: 0.375rem 0.75rem;
        background-color: white;
        height: calc(1.5em + 0.75rem + 2px);
        box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.075);
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
        box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
    }
</style>
