<template lang="">
    <div class="modal fade"
         :id="'modalFacture' + props.idFacture"
         data-bs-backdrop="static"
         data-bs-keyboard="false"
         tabindex="-1"
         aria-labelledby="staticBackdropLabel"
         aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content min-vh-75">
                <div class="modal-header">
                    <button type="button"
                            class="btn-close"
                            data-bs-dismiss="modal"
                            aria-label="Close"></button>
                </div>
                <div class="modal-body d-flex justify-content-center h-100">
                    <div class="container my-2" v-if="facture != ''">
                        <div class="d-flex align-items-center">
                            <h5 class="col-9">Les Encans de Nantes au Québec - FACTURE</h5>
                            <div class="col-3">
                                <img class="img-fluid" src="https://sqlinfocg.cegepgranby.qc.ca/2162067/images/Logo.png" />
                            </div>
                        </div>
                        <hr />
                        <div class="d-flex">
                            <div class="d-flex flex-column col-6 justify-content-between">
                                <h5>Facturé à</h5>
                                <span>{{facture.client.nom}}</span>
                                <span>{{facture.client.adresseLigne1}}</span>
                                <span>{{facture.client.adresseLigne2}}</span>
                                <span>{{facture.client.adresseLigne3}}</span>
                                <span>{{facture.client.codePostal}}</span>
                                <span>{{facture.client.courriel}}</span>
                                <span>{{facture.client.telephone}}</span>
                            </div>
                            <div class="col-6 d-flex align-items-end justify-content-start flex-column">
                                <h5>{{facture.dateAchat.split('T')[0]}}</h5>
                                <h5>{{facture.dateAchat.split('T')[1].split('.')[0]}}</h5>
                            </div>
                        </div>
                        <br />
                        <h4 class="text-center">
                            {{facture.prixFinal}} $ payé le {{facture.dateAchat.split('T')[0]}}
                        </h4>
                        <br />
                        <div class="d-flex w-100">
                            <table class="table">
                                <tbody>
                                    <tr v-for="lot in facture.lots">
                                        <th scope="col" class="text-start">{{lot.description}}</th>
                                        <td scope="col" class="text-end">{{lot.prix.toFixed(2)}} $</td>
                                    </tr>
                                    <tr>
                                        <th scope="col" class="text-start">Frais d'encanteur</th>
                                        <td scope="col" class="text-end">{{facture.fraisEncanteur.toFixed(2)}} $</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <br />
                        <div class="d-flex flex-column">
                            <table class="table col-6">
                                <tbody>
                                    <tr>
                                        <th scope="col" class="text-start">Sous-total</th>
                                        <td scope="col" class="text-end">{{facture.sousTotal.toFixed(2)}} $</td>
                                    </tr>
                                    <tr>
                                        <th scope="col" class="text-start">TPS</th>
                                        <td scope="col" class="text-end">{{facture.tps.toFixed(2)}} $</td>
                                    </tr>
                                    <tr>
                                        <th scope="col" class="text-start">TVQ</th>
                                        <td scope="col" class="text-end">{{facture.tvq.toFixed(2)}} $</td>
                                    </tr>
                                </tbody>
                            </table>
                            <h5 class="text-center">TOTAL: {{facture.prixFinal}} $</h5>
                        </div>
                    </div>
                    <div class="d-flex gap-2 justify-content-center" v-else>
                        <div class="spinner-border" role="status">
                            <span class="visually-hidden">Chargement des ventes...</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from "vue";
    import { useStore } from "vuex";

    const store = useStore();

    const props = defineProps({
        idFacture: Number
    });

    const facture = ref("")

    onMounted(async () => {
        const reponse = await store.dispatch("chercherDetailsFacture", props.idFacture)
        facture.value = reponse.data
    });
</script>
<style scoped>
    .min-vh-75 {
        min-height: 75vh
    }

    .min-vh-50 {
        min-height: 50vh
    }
</style>
