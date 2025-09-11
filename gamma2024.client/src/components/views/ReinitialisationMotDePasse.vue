<template>
    <div class="bg-image p-2 pt-md-5 imageDeFondEsquise">
        <transition name="fade" class="transit">
            <div class="w-100 d-flex justify-content-center">
                <div v-if="message"
                     :class="[
          'alert col-12 col-md-6',
          message.type === 'success' ? 'alert-success' : 'alert-danger',
        ]">
                    {{ message.text }}
                </div>
            </div>

        </transition>
        <div class="container d-flex flex-column justify-content-start align-items-stretch bg-white bg-opacity-75 cadreBlanc">
            <h2 class="fs-1 text-center fw-bold pt-4">
                Réinitialiser le mot de passe
            </h2>

            <form @submit.prevent="effacerMotDePasse"
                  class="d-flex flex-column justify-content-start align-items-stretch">
                <div class="px-4 py-2 mb-4">
                    <div class="d-flex flex-row justify-content-center mb-3">
                        <div class="form-group w-90">
                            <label for="email" class="fw-bold ms-3">Adresse courriel</label>
                            <input type="text"
                                   :class="['form-control', { 'is-invalid': vuelidate.email.$error }]"
                                   id="email"
                                   v-model="effacerDonneesMotDePasse.email"
                                   @blur="vuelidate.email.$touch()" />
                            <div class="retroaction-invalide" v-if="vuelidate.email.$error">
                                {{ vuelidate.email.$errors[0].$message }}
                            </div>
                        </div>
                    </div>
                    <div class="d-flex flex-row justify-content-center mb-3">
                        <div class="form-group w-90">
                            <label for="confirmPassword" class="fw-bold ms-3">Nouveau mot de passe</label>
                            <input type="password"
                                   :class="['form-control', { 'is-invalid': vuelidate.password.$error }]"
                                   id="confirmPassword"
                                   v-model="effacerDonneesMotDePasse.password"
                                   @blur="vuelidate.password.$touch()" />
                            <div class="retroaction-invalide" v-if="vuelidate.password.$error">
                                {{ vuelidate.password.$errors[0].$message }}
                            </div>
                        </div>
                    </div>
                    <div class="d-flex flex-row justify-content-center mb-3">
                        <div class="form-group w-90">
                            <label for="password" class="fw-bold ms-3">Confirmer le nouveau mot de passe</label>
                            <input type="password"
                                   :class="[
                  'form-control',
                  { 'is-invalid': vuelidate.confirmPassword.$error },
                ]"
                                   id="password"
                                   v-model="effacerDonneesMotDePasse.confirmPassword"
                                   @blur="vuelidate.confirmPassword.$touch()" />
                            <div class="retroaction-invalide" v-if="vuelidate.confirmPassword.$error">
                                {{ vuelidate.confirmPassword.$errors[0].$message }}
                            </div>
                        </div>
                    </div>
                    <div class="d-flex flex-row justify-content-center mb-2">
                        <button :disabled="estSoumis"
                                class="btn bleuNonValide rounded-pill px-5 text-white"
                                type="submit">
                            <span v-if="estSoumis"
                                  class="spinner-grow spinner-grow-sm"
                                  role="status"
                                  aria-hidden="true"></span>
                            Changer le mot de passe
                        </button>
                    </div>
                </div>
            </form>
        </div>
    </div>
</template>

<script setup>
    import { ref, computed, reactive } from "vue";
    import { useVuelidate } from "@vuelidate/core";
    import {
        required,
        email,
        helpers,
        sameAs,
        minLength,
    } from "@vuelidate/validators";
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";

    const store = useStore();
    const router = useRouter();
    let estSoumis = ref(false);
    const message = ref(null);
    const messageRequis = helpers.withMessage("Ce champ est requis.", required);
    const messageCourriel = helpers.withMessage(
        "Veuillez entrer un courriel valide.",
        email
    );

    const messageLongueurMinimaleMotDePasse = helpers.withMessage(
        "Le mot de passe doit contenir au moins 8 caractères, minimum 1 majuscule, 1 minuscule, 1 chiffre et 1 caractère spécial.",
        minLength(8)
    );

    let effacerDonneesMotDePasse = reactive({
        email: "",
        password: "",
        confirmPassword: "",
    });

    const messageIdentiqueMotDePasse = helpers.withMessage(
        "Les mots de passe ne correspondent pas.",
        sameAs(computed(() => effacerDonneesMotDePasse.password))
    );

    let regles = computed(() => {
        return {
            email: {
                required: messageRequis,
                email: messageCourriel,
            },
            password: {
                required: messageRequis,
                minLength: messageLongueurMinimaleMotDePasse,
            },
            confirmPassword: {
                required: messageRequis,
                sameAsPassword: messageIdentiqueMotDePasse,
            },
        };
    });

    const vuelidate = useVuelidate(regles, effacerDonneesMotDePasse);

    const effacerMotDePasse = async function () {
        const resultat = await vuelidate.value.$validate();
        estSoumis.value = true;

        if (!resultat) {
            estSoumis.value = false;
            message.value = {
                type: "danger",
                text: "Certaines informations du formulaire sont incorrectes",
            };
            setTimeout(() => {
                message.value = null;
            }, 3500);
            return;
        }

        try {
            const reponse = await store.dispatch(
                "reinitialisePassword",
                effacerDonneesMotDePasse
            );
            if (reponse.success) {
                message.value = { type: "success", text: reponse.message };

                setTimeout(() => {
                    router.push("/connexion");
                }, 4000);
            } else {
                message.value = { type: "danger", text: reponse.message };
            }
        } catch (erreur) {
            message.value = {
                type: "danger",
                text: "Une erreur est survenue lors de la création du compte.",
            };
        } finally {
            setTimeout(() => {
                message.value = null;
            }, 3500);
            estSoumis.value = false;
        }
    };
</script>

<style scped>
    .cadreBlanc {
        border-radius: 15px;
        min-height: 450px;
    }

    .w-90 {
        width: 90%;
    }

    .bleuNonValide {
        background-color: #052445;
        color: white;
    }

        .bleuNonValide:hover {
            background-color: #5a708a;
        }

    .text-danger {
        font-size: 0.875rem;
    }

    .retroaction-invalide {
        display: block;
        color: #dc3545;
        font-size: 0.875rem;
    }

    .lockMessage {
        margin: auto;
        width: 35%;
        text-align: center;
        margin-bottom: 10px;
    }

    .transit {
        margin: auto;
        margin-bottom: 15px;
    }

    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 2s ease;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }
</style>
