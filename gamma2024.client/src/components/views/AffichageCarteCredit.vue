<template>
    <div class="card">
        <div class="card-header">Carte de crédit ({{carte.marque}})</div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <label for="numeroCarte">Dernier 4 chiffres de la carte</label>
                    <div class="input-wrapper">
                        <input type="text"
                               id="numeroCarte"
                               disabled
                               class="form-control"
                               :value="'**********'+carte.dernier4Numero" />
                    </div>
                </div>
                <div class="col-md-6">
                    <label for="dateExpiration">Date d'expiration</label>
                    <div class="input-wrapper">
                        <input type="text"
                               mask="99/9999"
                               class="form-control"
                               placeholder="MM/YY"
                               id="dateExpiration"
                               disabled
                               v-model="carte.expirationDate" />
                    </div>
                </div>
            </div>
            <button v-if="estClient"
                    class="btn btn-danger mt-3"
                    data-bs-toggle="modal"
                    :data-bs-target="'#' + carte.paymentMethodId">
                Supprimer
            </button>
        </div>
    </div>
</template>
<script setup>
    import { ref, computed } from "vue";
    import { useStore } from "vuex";

    const store = useStore();

    const estClient = computed(() => store.getters.isClient)
    const props = defineProps({
        carte: Object
    });
</script>
<style></style>