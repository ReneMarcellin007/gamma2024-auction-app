<template>
    <div class="d-flex flex-column align-items-center pb-3">
        <h1>Encans passés</h1>

        <div class="d-flex gap-2" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des encans...</span>
            </div>
            <p>Chargement des encans en cours...</p>
        </div>
        <div v-else class="d-flex justify-content-center w-100">
            <h5 class="text-center" v-if="encans.length < 1">
                Il n'y a aucun encan passés pour le moment
            </h5>

            <div v-else class="w-100 px-3 row row-cols-lg-2 row-cols-1">
                <div v-for="index in encans" class="col py-3">
                    <router-link :to="{ name: 'Encan', params: { numeroEncan: index.numeroEncan } }"
                                 class="text-decoration-none text-black card_encan"
                                 @click.prevent="verifierEtRediriger(index)">
                        <AffichageEncanTuile :encan="index" />
                    </router-link>
                </div>
            </div>
        </div>

        <br />

        <router-link :to="{ name: 'TousLesEncans' }" class="text-black">
            <p>Voir tous les encans</p>
        </router-link>
    </div>
</template>

<script setup>
    import AffichageEncanTuile from "@/components/views/AffichageEncanTuile.vue";
    import { onMounted, ref } from "vue";
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";

    const store = useStore();
    const router = useRouter();
    const encans = ref([]);
    const chargement = ref(true);

    const verifierEtRediriger = async (encan) => {
        const maintenant = new Date();
        const dateDebutSoiree = new Date(encan.dateDebutSoireeCloture);

        // Vérifier si l'encan est en soirée de clôture
        const reponse = await store.dispatch('verifierEtatEncan');
        const type = reponse;

        if (type === 'soireeCloture' && encan.numeroEncan === store.state.encanCourant?.numeroEncan) {
            // Rediriger vers la soirée de clôture
            router.push({ name: 'EncanPresent' });
        } else {
            // Rediriger vers l'encan normal
            router.push({
                name: 'Encan',
                params: { numeroEncan: encan.numeroEncan }
            });
        }
    };

    onMounted(async () => {
        const reponse = await store.dispatch("chercherEncansPasses");
        encans.value = reponse.data;
        chargement.value = false;
    });
</script>

<style scoped></style>
