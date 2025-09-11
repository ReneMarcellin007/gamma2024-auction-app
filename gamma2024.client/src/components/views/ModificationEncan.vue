<template>
    <div class="d-flex flex-column justify-content-between">
        <h2 class="d-flex justify-content-center">Modification de l'encan</h2>

        <transition name="fade" class="transit">
            <div v-if="messageErreur" class="alert alert-danger">
                {{ messageErreur }}
            </div>
            <div v-else>
                <div v-if="messageSucces" class="alert alert-success">
                    {{ messageSucces }}
                </div>
            </div>
        </transition>

        <div class="d-flex justify-content-center mt-4">
            <form @submit.prevent="mettreAJourEncan" class="text-center">
                <div class="mb-3">
                    <label for="numeroEncan" class="form-label">Numéro de l'encan:</label>
                    <input type="number"
                           disabled
                           aria-label="numeroEncan"
                           v-model="encanData.numeroEncan"
                           :class="['form-control', { 'is-invalid': vuelidate.numeroEncan.$error }]"
                           @blur="vuelidate.numeroEncan.$touch()" />
                    <div class="invalid-feedback" v-if="vuelidate.numeroEncan.$error">
                        {{ vuelidate.numeroEncan.$errors[0].$message }}
                    </div>
                </div>
                <div class="mb-3">
                    <label for="dateDebut" class="form-label">Date de début:</label>
                    <VueDatePicker type="date"
                                   v-model="encanData.dateDebut"
                                   :class="['form-control', { 'is-invalid': vuelidate.dateDebut.$error }]"
                                   @blur="vuelidate.dateDebut.$touch()"
                                   :min-date="new Date()"
                                   :max-date="desacDateDebutEntre"
                                   :enable-time-picker="false"
                                   select-text="Choisir"
                                   cancel-text="Annuler"
                                   now-button-label="Aujourd'hui"
                                   :action-row="{ showNow: true }"
                                   :format-locale="fr" />
                    <div class="invalid-feedback" v-if="vuelidate.dateDebut.$error">
                        {{ vuelidate.dateDebut.$errors[0].$message }}
                    </div>
                </div>
                <div class="mb-3">
                    <label for="dateFin" class="form-label">Date de fin:</label>
                    <VueDatePicker type="date"
                                   v-model="encanData.dateFin"
                                   :class="['form-control', { 'is-invalid': vuelidate.dateFin.$error }]"
                                   @blur="vuelidate.dateFin.$touch()"
                                   :min-date="desacDateFinEntre"
                                   :enable-time-picker="true"
                                   select-text="Choisir"
                                   cancel-text="Annuler"
                                   :format-locale="fr" />
                    <div class="invalid-feedback" v-if="vuelidate.dateFin.$error">
                        {{ vuelidate.dateFin.$errors[0].$message }}
                    </div>
                </div>

                <div class="mb-3 d-flex flex-column">
                    <label for="pasLot" class="form-label">Délai de base de chaque lot (seconde):</label>
                    <input type="text"
                           @blur="vuelidate.pasLot.$touch()"
                           aria-label="pasLot"
                           class="bordureGrise"
                           min="1"
                           v-model="encanData.pasLot" />
                    <div class="invalid-feedback d-inline" v-if="vuelidate.pasLot.$error">
                        {{ vuelidate.pasLot.$errors[0].$message }}
                    </div>
                </div>

                <div class="mb-3 d-flex flex-column">
                    <label for="pasMise" class="form-label">Temps ajouté (seconde) lors d'une mise (­< 60 sec) :</label>
                    <input type="search"
                           @blur="vuelidate.pasMise.$touch()"
                           aria-label="Recherche"
                           class="bordureGrise"
                           min="1"
                           v-model="encanData.pasMise" />
                    <div class="invalid-feedback d-inline" v-if="vuelidate.pasMise.$error">
                        {{ vuelidate.pasMise.$errors[0].$message }}
                    </div>
                </div>

                <button type="button"
                        @click="effaceFormulaire"
                        class="btn btn-secondary me-3"
                        :disabled="!changementFormulaire">
                    Réinitialiser
                </button>
                <button type="submit"
                        :disabled="!changementFormulaire"
                        :class="[changementFormulaire ? 'bleuValide' : 'bleuNonValide', 'btn ']">
                    Enregistrer
                </button>
            </form>
        </div>
    </div>
</template>

<script setup>
    import { ref, reactive, computed, onMounted, watch } from "vue";
    import useVuelidate from "@vuelidate/core";
    import { required, helpers, minValue } from "@vuelidate/validators";
    import { useStore } from "vuex";
    import { useRoute, useRouter } from "vue-router";
    import VueDatePicker from "@vuepic/vue-datepicker";
    import "@vuepic/vue-datepicker/dist/main.css";
    import { fr } from "date-fns/locale";

    const route = useRoute();
    const router = useRouter();
    const store = useStore();

    const messageRequis = helpers.withMessage(
        "Ce champ est obligatoire.",
        required
    );
    let messageErreur = ref("");
    let messageSucces = ref("");
    let changementFormulaire = ref(false);
    let formulaireInitial = ref({});

    const valeurMinimum = helpers.withMessage(
        "Ce champ doit être supérieur à 1 seconde",
        minValue(1)
    );

    let encanData = reactive({
        id: "",
        numeroEncan: "",
        dateDebut: "",
        dateFin: "",
        pasLot: "",
        pasMise: "",
    });

    onMounted(async () => {
        const reponse = await store.dispatch("obtenirUnEncanParId", route.params.id);
        if (reponse) {
            encanData.id = route.params.id;
            encanData.numeroEncan = reponse.numeroEncan;
            encanData.dateDebut = reponse.dateDebut;
            encanData.dateFin = reponse.dateFin;
            encanData.pasLot = reponse.pasLot;
            encanData.pasMise = reponse.pasMise;
        }

        formulaireInitial.value = JSON.parse(JSON.stringify(encanData));
        changementFormulaire.value = false;
    });

    let regles = computed(() => {
        return {
            numeroEncan: { required: messageRequis },
            dateDebut: { required: messageRequis },
            dateFin: { required: messageRequis },
            pasLot: { required: messageRequis, minValueValue: valeurMinimum },
            pasMise: { required: messageRequis, minValueValue: valeurMinimum }
        };
    });

    const vuelidate = useVuelidate(regles, encanData);

    const desacDateFinEntre = computed(() => {
        const debutDate = encanData.dateDebut;

        if (debutDate != "") {
            const dateDebutDesac = new Date(debutDate);
            dateDebutDesac.setDate(dateDebutDesac.getDate() + 1);

            return [dateDebutDesac];
        }
        return null;
    });

    const desacDateDebutEntre = computed(() => {
        const finDate = encanData.dateFin;

        if (finDate != "") {
            const dateFinDesac = new Date(finDate);
            dateFinDesac.setDate(dateFinDesac.getDate() - 1);

            return [dateFinDesac];
        }
        return null;
    });

    watch(
        () => JSON.parse(JSON.stringify(encanData)), // Deep clone copy to avoid same ref type.
        () => {
            // Comparaison entre l'état initial et l'état actuel
            if (JSON.stringify(encanData) !== JSON.stringify(formulaireInitial.value)) {
                changementFormulaire.value = true;
            } else {
                changementFormulaire.value = false;
            }
        },
        { deep: true }
    );

    const effaceFormulaire = function () {
        Object.assign(encanData, JSON.parse(JSON.stringify(formulaireInitial.value)));
        vuelidate.value.$reset();
        changementFormulaire.value = false;
    };

    const mettreAJourEncan = async function () {
        try {
            const resultat = await store.dispatch("modifierEncan", encanData);
            if (resultat.success) {
                messageSucces.value = resultat.message;
                setTimeout(() => {
                    router.push({ name: "TableauDeBordEncans" });
                }, 3000);
            } else {
                messageErreur.value = resultat.error;
                setTimeout(() => {
                    messageErreur.value = "";
                }, 4000);
            }
        } catch (erreur) {
            console.error("Erreur lors de la modification de l'encan:", erreur);
        }
    };
</script>

<style scoped>
    .bleuValide {
        background-color: #052445;
        color: white;
    }

    .bleuNonValide {
        background-color: #5a708a;
        color: white;
    }

    .transit {
        margin: auto;
        width: 600px;
    }

    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 3s ease;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }
</style>
