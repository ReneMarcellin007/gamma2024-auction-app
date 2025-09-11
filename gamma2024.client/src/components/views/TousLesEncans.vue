<template>
    <div class="d-flex flex-column align-items-center pb-3">
        <h1>
            Tous les encans
        </h1>

        <div class="d-flex gap-2" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des encans...</span>
            </div>
            <p>Chargement des encans en cours...</p>
        </div>
        <div v-else class="d-flex justify-content-center w-100">

            <h5 class="text-center" v-if="encans.length <1">Il n'y a aucun encan pour le moment</h5>

            <div v-else class="w-100 px-3 row row-cols-lg-2 row-cols-1">
                <div v-for="index in encans" class="col py-3">
                    <span @click="voirEncan(index.numeroEncan)" role="button">
                        <AffichageEncanTuile :encan="index" />
                    </span>
                </div>
            </div>

        </div>
    </div>
</template>

<script setup>
    import AffichageEncanTuile from '@/components/views/AffichageEncanTuile.vue'
    import { onMounted, ref } from 'vue'
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";

    const store = useStore();
    const router = useRouter()
    const encans = ref([])
    const chargement = ref(true)
    const numEncanCours = ref(0)

    const voirEncan = ref(function (numeroEncanRecu) {
        if (numeroEncanRecu == numEncanCours.value) {
            router.push({ name: 'EncanPresent' })
        }
        else {
            router.push({
                name: 'Encan', params: { numeroEncan: numeroEncanRecu }
            })
        }
    })

    onMounted(async () => {
        const reponse = await store.dispatch("chercherTousEncansVisibles");
        encans.value = reponse.data

        const reponseNumEncanCourrant = await store.dispatch("chercherNumeroEncanEnCours");
        if (reponseNumEncanCourrant.data != '') {
            numEncanCours.value = reponseNumEncanCourrant.data
        }
        chargement.value = false
    })
</script>

<style scoped>
</style>