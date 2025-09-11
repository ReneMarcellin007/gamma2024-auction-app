<template>
    <div class="bg-image p-2 pt-md-5 imageDeFondEsquise h-100">
        <transition name="fade">
            <div v-if="messageVerrouiller"
                 class="container lockMessage alert alert-warning"
                 role="alert">
                {{ messageVerrouiller }}
            </div>
        </transition>
        <div class="container d-flex flex-column justify-content-start align-items-stretch bg-white bg-opacity-75 cadreBlanc">
            <h2 class="fs-1 text-center fw-bold pt-4">Connexion</h2>
            <p class="text-center">Se connecter pour continuer</p>

            <!-- Ajoutez cette section pour afficher les messages d'erreur -->
            <div v-if="messageErreur"
                 class="alert alert-danger text-center"
                 role="alert">
                {{ messageErreur }}
            </div>

            <form @submit.prevent="connexion"
                  class="d-flex flex-column justify-content-start align-items-stretch">
                <div class="px-4 py-2 mb-4">
                    <div class="d-flex flex-row justify-content-center mb-3">
                        <div class="form-group w-90">
                            <label for="emailOuPseudo" class="fw-bold ms-3">Email ou pseudonyme</label>
                            <input type="text"
                                   class="form-control"
                                   id="emailOuPseudo"
                                   v-model="emailOuPseudo"
                                   @input="validerEmailOuPseudo"
                                   :class="{ 'is-invalid': emailOuPseudoErreur }" />
                            <div v-if="emailOuPseudoErreur" class="retroaction-invalide">
                                {{ emailOuPseudoErreur }}
                            </div>
                        </div>
                    </div>
                    <div class="d-flex flex-row justify-content-center mb-3">
                        <div class="form-group w-90">
                            <label for="motdepasse" class="fw-bold ms-3">Mot de passe</label>
                            <input type="password"
                                   class="form-control"
                                   id="motDePasse"
                                   v-model="motDePasse"
                                   @input="validerMotDePasse"
                                   :class="{ 'is-invalid': motDePasseErreur }" />
                            <div v-if="motDePasseErreur" class="retroaction-invalide">
                                {{ motDePasseErreur }}
                            </div>
                        </div>
                    </div>
                    <div class="d-flex flex-row justify-content-center mt-4 mb-1">
                        <div class="form-group w-80">
                            <router-link :to="{ name: 'ReinitialiserMotDePasse' }"
                                         class="text-decoration-none">
                                <p class="text-center lienEnTexteNoir">
                                    J'ai oublié mon Mot de passe
                                </p>
                            </router-link>
                        </div>
                    </div>
                    <div class="d-flex flex-row justify-content-center mb-2">
                        <button :disabled="isSubmitting"
                                class="btn bleuNonValide rounded-pill px-5 text-white"
                                @click="connexion">
                            <span v-if="isSubmitting"
                                  class="spinner-grow spinner-grow-sm"
                                  role="status"
                                  aria-hidden="true"></span>
                            Connexion
                        </button>
                    </div>
                    <div class="d-flex flex-row justify-content-center mb-3">
                        <div class="form-group w-80">
                            <router-link :to="{ name: 'Inscription' }"
                                         class="text-decoration-none">
                                <p class="fs-5 text-center fw-bold lienEnTexteNoir">
                                    M'inscrire
                                </p>
                            </router-link>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                emailOuPseudo: "",
                motDePasse: "",
                emailOuPseudoErreur: "",
                motDePasseErreur: "",
                messageVerrouiller: "",
                messageErreur: "",
                isSubmitting: false,
                invalide: false,
            };
        },

        computed: {
            isValide() {
                if (this.emailOuPseudo.trim() !== "" && this.motDePasse.trim() !== "") {
                    return true;
                }
                return false;
            },
        },

        methods: {
            validerEmailOuPseudo() {
                if (this.emailOuPseudo.trim() === "") {
                    this.emailOuPseudoErreur =
                        "L'email ou le pseudonyme est requis pour la connexion";
                    return;
                }
                this.emailOuPseudoErreur = "";
            },
            validerMotDePasse() {
                if (this.motDePasse.trim() === "") {
                    this.motDePasseErreur = "Le mot de passe est requis pour la connexion";
                    return;
                }
                this.motDePasseErreur = "";
            },

            async connexion() {
                this.isSubmitting = true;
                this.validerEmailOuPseudo();
                this.validerMotDePasse();

                if (!this.isValide) {
                    this.isSubmitting = false;
                    return;
                }

                try {
                    let result = await this.$store.dispatch("login", {
                        emailOuPseudo: this.emailOuPseudo,
                        password: this.motDePasse,
                    });
                    if (result.success) {
                        // Vérifiez la structure de result.roles
                        const rolesString = Array.isArray(result.roles)
                            ? result.roles.join(", ")
                            : result.roles && result.roles
                                ? result.roles.join(", ")
                                : "Rôles non disponibles";

                        this.messageSucces = `Connexion réussie en tant que ${rolesString}`;
                        // Redirection immédiate vers la page d'accueil
                        if (rolesString == 'Administrateur') {
                            this.$router.push({ name: 'Accueil' });
                        }
                        else if (rolesString == 'Client') {

                            let reponse = await this.$store.dispatch("chercherCartesUser");

                            if (reponse.data.length < 1) {
                                this.$router.push({ name: 'EnregistrerCarte' });
                            }
                            else {
                                this.$router.push({ name: 'EncanPresent' });
                            }

                        }
                        else {
                            this.$router.push({ name: 'Accueil' })
                        }


                    } else {
                        if (result.element == "password") {
                            this.motDePasseErreur = result.error;
                        } else if (result.element === "lock") {
                            this.messageVerrouiller = result.error;
                            setTimeout(() => {
                                this.messageVerrouiller = "";
                            }, 5000);
                        } else {
                            this.emailOuPseudoErreur = result.error;
                        }
                    }
                } catch (erreur) {
                    if (erreur.response && erreur.response.data) {
                        const { element, message } = erreur.response.data;
                        if (element === "password") {
                            this.motDePasseErreur = message;
                        } else if (element === "lock") {
                            this.messageVerrouiller = message;
                            setTimeout(() => {
                                this.messageVerrouiller = "";
                            }, 5000);
                        } else if (element === "user_pseudo") {
                            this.emailOuPseudoErreur = message;
                        } else {
                            this.messageErreur = message || "Une erreur est survenue lors de la connexion.";
                        }
                    } else {
                        this.messageErreur = "Une erreur est survenue lors de la connexion.";
                    }
                } finally {
                    this.isSubmitting = false;
                }
            },
        },
    };
</script>

<style scoped>
    .cadreBlanc {
        border-radius: 15px;
        min-height: 450px;
        width: 410px;
    }

    .imageDeFondEsquise {
        background-image: url("/images/DessinGris.png");
        background-size: cover;
        background-position: 0px -100px;
        background-attachment: fixed;
        height: 100vh;
        width: 100%;
    }

    .w-80 {
        width: 80%;
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

    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 1s ease;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }
</style>
