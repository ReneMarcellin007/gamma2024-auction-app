<template>
    <span v-for="carte in cartes">
        <ConfirmationSuppression :pmId="carte.paymentMethodId" :dernier4Numero="carte.dernier4Numero" :marque="carte.marque" @supprimerCarte="supprimerCarte"></ConfirmationSuppression>
    </span>
    <div class="container">
        <h1 class="text-center">Gestion des cartes de crédit</h1>

        <div class="alert alert-danger mx-3" role="alert" id="message" v-show="siMessage">
        </div>
        <div class="alert alert-success mx-3" role="alert" id="messageSuccess" v-show="siMessageSucces">
        </div>

        <div v-if="chargement" class=text-center>
            <div class="spinner-border" role="status">
                <span class="sr-only"></span>
            </div>
        </div>
        <div v-else>
            <div class="d-flex justify-content-center justify-content-md-start">
                <router-link :to="{name: 'EnregistrerCarte'}">
                    <button class="btn btn-outline text-white btnSurvolerBleuMoyenFond my-2 mx-3">
                        Ajouter une carte
                    </button>
                </router-link>
            </div>

            <div v-if="cartes.length == 0" class="text-center">
                <h5>Aucun carte trouvé. Afin d'utiliser nos services, vous devez avoir au moins une carte sur votre profil.</h5>
            </div>
            <div class="d-flex flex-wrap w-100 justify-content-between" v-else>
                <div v-for="carte in cartes" class="col-12 col-md-6 p-3">
                    <AffichageCarteCredit :carte="carte"></AffichageCarteCredit>
                </div>
            </div>

        </div>

    </div>

</template>
<script setup>
    import { useStore } from "vuex";
    import AffichageCarteCredit from '@/components/views/AffichageCarteCredit.vue'
    import ConfirmationSuppression from "@/components/modals/ConfirmationSuppressionCarte.vue";
    import { ref, onMounted } from "vue";
    const store = useStore();
    const cartes = ref([])
    const chargement = ref(true)
    const siMessage = ref(false)
    const siMessageSucces = ref(false)

    onMounted(async () => {
        try {
            const reponse = await store.dispatch("chercherCartesUser")
            cartes.value = reponse.data
            chargement.value = false
        } catch (error) {
            console.error("Erreur lors de la récupération des lots:", error);
        }
    });

    const supprimerCarte = ref(async function (pmId) {
        try {
            if (cartes.value.length > 1) {
                siMessage.value = false
                const carteSuppression = await store.dispatch("supprimerCarte", pmId)
                const reponse = await store.dispatch("chercherCartesUser")
                cartes.value = reponse.data
                siMessageSucces.value = true
                siMessage.value = false
                document.querySelector("#messageSuccess").innerHTML = "Carte supprimé avec succès."
            }
            else {
                siMessage.value = true
                siMessageSucces.value = false
                document.querySelector("#message").innerHTML = "Vous devez avoir au moins une carte dans votre profil afin d'utiliser nos services"
            }

        } catch (error) {
            siMessage.value = true
            siMessageSucces.value = false
            document.querySelector("#message").innerHTML = error
        }
    })
</script>
<style></style>