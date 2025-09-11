<template lang="">
  <!-- Modal -->
  <div
    class="modal fade"
    :id="props.h.numeroEncan"
    data-bs-backdrop="static"
    data-bs-keyboard="false"
    tabindex="-1"
    aria-labelledby="staticBackdropLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h1 class="modal-title fs-5" id="staticBackdropLabel">
            Confirmation de suppression
          </h1>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          Êtes-vous sur de vouloir supprimer l'encan {{ props.h.numeroEncan }}
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
          >
            Annuler
          </button>
          <button
            type="button"
            class="btn btn-primary"
            @click="ConfirmationDelete(props.h.numeroEncan)"
            data-bs-dismiss="modal"
          >
            Confirmer
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  h: {
    type: Object,
    required: true,
  },
});

const emit = defineEmits(["supprimerEncan"]);

const ConfirmationDelete = function (encanNumero) {
  emit("supprimerEncan", encanNumero);
};
</script>
<style scoped></style>
