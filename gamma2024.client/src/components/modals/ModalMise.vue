<template>
    <div class="modal fade"
         :id="`modalMise_${lot?.id}`"
         tabindex="-1"
         :aria-labelledby="`modalMiseLabel_${lot?.id}`"
         aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" :id="`modalMiseLabel_${lot?.id}`">
                        Êtes-vous sûr de faire cette mise?
                    </h5>
                    <button type="button"
                            class="btn-close"
                            data-bs-dismiss="modal"
                            aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="d-flex flex-column gap-3">
                        <p class="fs-5 mb-0">Lot {{ lot?.numero }}</p>

                        <div class="mise-info">
                            <p class="fs-5 mb-2">Mise actuelle : {{ formatMontant(lot?.mise || lot?.prixOuverture) }}$</p>
                            <p class="fs-5 mb-2">Votre mise : {{ formatMontant(getMiseAffichage) }}$</p>
                        </div>

                        <div class="d-flex flex-column gap-2">
                            <div class="form-check form-switch mb-2">
                                <input class="form-check-input"
                                       type="checkbox"
                                       v-model="miseAutomatique"
                                       id="switchMiseAuto">
                                <label class="form-check-label" for="switchMiseAuto">
                                    Activer la mise automatique
                                </label>
                            </div>

                            <div v-if="miseAutomatique" class="d-flex align-items-center gap-2">
                                <button class="btn btn-outline-secondary"
                                        @click="decrementerMise"
                                        :disabled="montantMise <= getMiseMinimale">
                                    -{{ pasEnchere }}$
                                </button>
                                <span class="fs-5">{{ montantMise }}$</span>
                                <button class="btn btn-outline-secondary"
                                        @click="incrementerMise">
                                    +{{ pasEnchere }}$
                                </button>
                            </div>
                            <p v-if="miseAutomatique" class="text-info small">
                                Votre mise commencera à {{ getMiseMinimale }}$ et augmentera automatiquement jusqu'à {{ montantMise }}$
                            </p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-secondary"
                            data-bs-dismiss="modal">
                        Annuler
                    </button>
                    <button type="button"
                            class="btn btn-primary"
                            @click="confirmerMise"
                            :disabled="!isMiseValide">
                        Miser
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, watch, onMounted, computed, nextTick, h } from "vue";
    import { useStore } from "vuex";
    import { toast } from "vue3-toastify";
    import ToastContent from "../Toast/toastConfirm.vue";

    const bootstrap = window.bootstrap;

    const store = useStore();
    const modalInstance = ref(null);
    const montantMise = ref(0);
    const montantInitial = ref(0);
    const utilisateurDerniereMise = ref(0);
    const miseAutomatique = ref(false);

    const props = defineProps({
        lot: {
            type: Object,
            required: true,
        },
    });

    // Fonction pour calculer le pas d'enchère selon le montant
    const calculerPasEnchere = (montant) => {
        if (montant <= 199.0) return 10.0;
        if (montant <= 499.0) return 25.0;
        if (montant <= 999.0) return 50.0;
        if (montant <= 1999.0) return 100.0;
        if (montant <= 4999.0) return 200.0;
        if (montant <= 9999.0) return 250.0;
        if (montant <= 19999.0) return 500.0;
        if (montant <= 49999.0) return 1000.0;
        if (montant <= 99999.0) return 2000.0;
        if (montant <= 499999.0) return 5000.0;
        return 10000.0;
    };

    // Computed pour le pas d'enchère actuel
    const pasEnchere = computed(() => {
        return calculerPasEnchere(montantMise.value);
    });

    // Computed pour la mise minimale selon le type de mise
    const getMiseMinimale = computed(() => {
        const miseActuelle = props.lot?.mise || 0;
        const prixOuverture = props.lot?.prixOuverture;

        // S'il n'y a pas de mise, on utilise le prix d'ouverture
        if (miseActuelle === 0) {
            if (miseAutomatique.value) {
                return prixOuverture;
            }
            return prixOuverture;
        }

        // S'il y a déjà une mise
        if (miseAutomatique.value) {
            return miseActuelle + calculerPasEnchere(miseActuelle);
        }
        return miseActuelle + calculerPasEnchere(miseActuelle);
    });

    // Computed pour valider la mise
    const isMiseValide = computed(() => {
        return montantMise.value >= getMiseMinimale.value;
    });

    // Calcul du montant initial basé sur le lot
    const calculerMontantInitial = (lot) => {
        if (!lot) return 0;
        return lot.mise || lot.prixOuverture || 0;
    };

    // Confirmer la mise
    const confirmerMise = async () => {
        try {
            // Vérifications préalables
            if (props.lot.estVendu) {
                toast.error(
                    h(ToastContent, {
                        title: "Erreur",
                        description: "Ce lot est déjà vendu",
                    })
                );
                return;
            }

            if (props.lot.idClientMise === store.state.user?.id) {
                toast.info(
                    h(ToastContent, {
                        title: "Information",
                        description: "Vous êtes déjà le plus haut enchérisseur",
                    })
                );
                return;
            }

            let montantEffectif;
            let montantMaximal = null;

            const miseActuelle = props.lot.mise || 0;
            const prixOuverture = props.lot.prixOuverture;

            if (miseAutomatique.value) {
                // Mode mise automatique
                montantMaximal = montantMise.value; // Le montant maximum choisi par l'utilisateur
                // La mise effective commence au minimum (prix d'ouverture ou mise actuelle + step)
                montantEffectif = miseActuelle === 0 ?
                    prixOuverture :
                    miseActuelle + calculerPasEnchere(miseActuelle);
            } else {
                // Mode mise manuelle
                // La mise effective est directement le prochain palier
                montantEffectif = miseActuelle === 0 ?
                    prixOuverture + calculerPasEnchere(prixOuverture) :
                    miseActuelle + calculerPasEnchere(miseActuelle);
                montantMaximal = null; // Pas de maximum en mode manuel
            }

            const miseData = {
                lotId: props.lot.id,
                montant: montantEffectif,
                montantMaximal: montantMaximal
            };

            const reponse = await store.dispatch("placerMise", miseData);

            if (reponse.success) {
                modalInstance.value.hide();
                emit("miseConfirmee", montantMise.value);

                toast.success(
                    h(ToastContent, {
                        title: "Succès",
                        description: miseAutomatique.value ?
                            `Mise automatique configurée : ${montantEffectif}$ (max: ${montantMaximal}$)` :
                            `Mise placée : ${montantEffectif}$`,
                    })
                );
            } else {
                // Afficher le message d'erreur
                toast.error(
                    h(ToastContent, {
                        title: "Erreur",
                        description: reponse.message
                    })
                );
            }
        } catch (error) {
            toast.error(
                h(ToastContent, {
                    title: "Erreur",
                    description: "Une erreur inattendue s'est produite"
                })
            );
        }
    };



    // Gestion du modal
    onMounted(() => {
        nextTick(async () => {
            const modalElement = document.getElementById(`modalMise_${props.lot?.id}`);
            if (modalElement) {
                modalInstance.value = new bootstrap.Modal(modalElement, {
                    backdrop: "static",
                    keyboard: false,
                });
            }
            if (props.lot?.id) {
                utilisateurDerniereMise.value = await store.dispatch('getUserBidForLot', props.lot.id);
            }
        });
    });

    // Méthodes pour la mise modifiées
    const incrementerMise = () => {
        const nouveauMontant = montantMise.value + pasEnchere.value;
        montantMise.value = nouveauMontant;
    };

    const decrementerMise = () => {
        const pasActuel = calculerPasEnchere(montantMise.value);
        const nouveauMontant = montantMise.value - pasActuel;
        if (nouveauMontant >= getMiseMinimale.value) {
            montantMise.value = nouveauMontant;
        }
    };

    // Méthodes du modal
    const presenterModal = () => {
        if (modalInstance.value) {
            const lot = store.getters.getLot(props.lot?.id);
            montantMise.value = miseAutomatique.value ?
                (lot?.mise || lot?.prixOuverture) + (calculerPasEnchere(lot?.mise || lot?.prixOuverture) * 2) :
                getMiseMinimale.value;
            miseAutomatique.value = false; // Réinitialiser le switch
            modalInstance.value.show();
        }
    };

    const cacherModal = () => {
        if (modalInstance.value) {
            modalInstance.value.hide();
        }
    };

    // Computed properties
    const affichageMiseActuelle = computed(() => {
        if (utilisateurDerniereMise.value > 0) {
            const lot = store.getters.getLot(props.lot?.id);
            const estPlusHautEncherisseur = lot?.idClientMise === store.state.user?.id;
            return `${utilisateurDerniereMise.value}$ ${estPlusHautEncherisseur ? '(meilleure offre)' : '(dépassée)'}`;
        }
        return "aucune mise";
    });

    const hasUserBidOnLot = computed(() => {
        return store.getters.hasUserBidOnLot(props.lot?.id);
    });

    // Expose les méthodes nécessaires
    defineExpose({
        presenterModal,
        cacherModal,
    });

    // Définir les émissions
    const emit = defineEmits(["miseConfirmee"]);

    // Watch pour mettre à jour le montant initial quand la mise change
    watch(
        () => store.getters.getLot(props.lot?.id)?.mise,
        async (newMise) => {
            if (newMise) {
                montantMise.value = newMise;
                utilisateurDerniereMise.value = await store.dispatch('getUserBidForLot', props.lot.id);
            }
        }
    );

    // Watch pour mettre à jour le montant quand on change le type de mise
    watch(miseAutomatique, (newValue) => {
        const miseActuelle = props.lot?.mise || 0;
        const prixOuverture = props.lot?.prixOuverture;

        if (newValue) { // Si on active la mise auto
            // Si pas de mise, on ajoute 2 steps au prix d'ouverture
            if (miseActuelle === 0) {
                montantMise.value = prixOuverture + (calculerPasEnchere(prixOuverture) * 2);
            } else {
                // Sinon on ajoute 2 steps à la mise actuelle
                montantMise.value = miseActuelle + (calculerPasEnchere(miseActuelle) * 2);
            }
        } else {
            montantMise.value = getMiseMinimale.value;
        }
    });

    // Nouveau computed pour l'affichage de la mise
    const getMiseAffichage = computed(() => {
        if (miseAutomatique.value) {
            return montantMise.value;
        } else {
            const miseActuelle = props.lot?.mise || 0;
            if (miseActuelle === 0) {
                return props.lot?.prixOuverture + calculerPasEnchere(props.lot?.prixOuverture);
            }
            return miseActuelle + calculerPasEnchere(miseActuelle);
        }
    });

    // Fonction utilitaire pour formater les montants
    const formatMontant = (montant) => {
        return montant ? Math.floor(montant) : 0;
    };
</script>

<style scoped>
    .modal-body {
        padding: 1.5rem;
    }

    .gap-3 {
        gap: 1rem !important;
    }

    .gap-2 {
        gap: 0.75rem !important;
    }

    .form-switch {
        padding-left: 2.5em;
    }

    .form-check-input {
        cursor: pointer;
    }
</style>
