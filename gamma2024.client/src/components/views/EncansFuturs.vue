<template>
    <div class="d-flex flex-column align-items-center pb-3">
        <h1>
            Encans futurs
        </h1>

        <div class="d-flex gap-2" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des encans...</span>
            </div>
            <p>Chargement des encans en cours...</p>
        </div>
        <div v-else class="d-flex justify-content-center w-100">

            <h5 class="text-center" v-if="encans.length <1">Il n'y a aucun encan futur pour le moment</h5>

            <div v-else class="w-100 px-3 row row-cols-lg-2 row-cols-1">
                <div v-for="index in encans" class="col py-3">
                    <router-link :to="{ name: 'Encan', params: { numeroEncan: index.numeroEncan }}" class="text-decoration-none text-black">
                        <AffichageEncanTuile :encan="index" />
                    </router-link>
                </div>
            </div>

        </div>

        <br />

        <router-link :to="{ name: 'TousLesEncans'}" class="text-black">
            <p>Voir tous les encans</p>
        </router-link>
    </div>
</template>

<script setup>
    import AffichageEncanTuile from '@/components/views/AffichageEncanTuile.vue'
    import { onMounted, ref } from 'vue'
    import { useStore } from "vuex";

    const store = useStore();
    const encans = ref([])
    const chargement = ref(true)

    onMounted(async () => {
        const reponse = await store.dispatch("chercherEncansFuturs");
        encans.value = reponse.data
        chargement.value = false
    })
</script>

<style scoped>
</style>