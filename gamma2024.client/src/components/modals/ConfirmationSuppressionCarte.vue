<template lang="">
    <!-- Modal -->
    <div class="modal fade"
         :id="props.pmId"
         data-bs-backdrop="static"
         data-bs-keyboard="false"
         tabindex="-1"
         aria-labelledby="staticBackdropLabel"
         aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="staticBackdropLabel">
                        Confirmation de suppression
                    </h1>
                    <button type="button"
                            class="btn-close"
                            data-bs-dismiss="modal"
                            aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    Êtes-vous sûr de vouloir supprimer cette carte ({{props.marque.toUpperCase()}} {{props.dernier4Numero}})?
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-secondary"
                            data-bs-dismiss="modal">
                        Annuler
                    </button>
                    <button type="button"
                            class="btn btn-primary"
                            @click="ConfirmationSuppression(props.pmId)"
                            data-bs-dismiss="modal">
                        Confirmer
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    const props = defineProps({
        pmId: String,
        marque: String,
        dernier4Numero: String
    });

    const emit = defineEmits(["supprimerCarte"]);

    const ConfirmationSuppression = function (pmId) {
        emit("supprimerCarte", pmId);
    };
</script>
<style scoped></style>
