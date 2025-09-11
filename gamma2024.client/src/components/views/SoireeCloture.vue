<template>
    <div class="soiree-cloture" v-if="encan && lotsTriesParTemps.length > 0">
        <h2 class="text-center w-100">Soirée de clôture - Encan #{{ encan.numeroEncan }}</h2>

        <div v-if="lotsRestants.length > 0">
            <!-- Les 15 premiers lots en tuiles -->
            <div class="row row-cols-lg-5 row-cols-md-3 row-cols-sm-2 row-cols-1 w-100 px-3">
                <div v-for="lot in premierQuinzeLots"
                     :key="lot.id"
                     class="col p-2 d-flex">
                    <LotTuile :lotRecu="lot"
                              :showDecompte="true" />
                </div>
            </div>

            <!-- Le reste en liste -->
            <div class="d-flex flex-column px-5 w-100">
                <div v-for="lot in autresLots"
                     :key="lot.id"
                     class="p-2">
                    <LotListe :lotRecu="lot"
                              :showDecompte="true" />
                </div>
            </div>
        </div>
        <div v-else>
            <h5 class="text-center">
                Tous les lots ont été vendus
            </h5>
        </div>
    </div>
</template>

<script setup>
    import { ref, computed, onMounted, watch } from 'vue'
    import { useStore } from 'vuex'
    import { useRouter } from 'vue-router'
    import LotTuile from '@/components/views/LotTuile.vue'
    import LotListe from '@/components/views/LotListe.vue'

    const store = useStore()
    const router = useRouter()
    const encan = computed(() => store.state.encanCourant)

    const lots = computed(() => {
        return store.state.lots || {}
    })

    const lotsTriesParTemps = computed(() => {
        return Object.values(lots.value)
            .filter(lot => {
                return !lot.estVendu &&
                    lot.dateFinDecompteLot &&
                    new Date(lot.dateFinDecompteLot) > new Date();
            })
            .sort((a, b) => {
                return new Date(a.dateFinDecompteLot) - new Date(b.dateFinDecompteLot);
            });
    })

    const premierQuinzeLots = computed(() => lotsTriesParTemps.value.slice(0, 15))
    const autresLots = computed(() => lotsTriesParTemps.value.slice(15))

    const lotsRestants = computed(() =>
        Object.values(lots.value).filter(lot => !lot.estVendu)
    )

    // Si tous les lots sont vendus, rediriger vers EncanPresent
    watch(lotsRestants, async (nouveauxLots) => {
        if (nouveauxLots.length === 0) {
            await router.push({ name: 'EncanPresent' })
        }
    })

    onMounted(async () => {
        const type = await store.dispatch('verifierEtatEncan')

        if (!type || type !== 'soireeCloture') {
            router.push({ name: 'EncanPresent' })
        }
    })

    // Ajouter un watch pour s'assurer que les lots sont chargés
    watch(() => encan.value, async (nouvelEncan) => {
        if (nouvelEncan && nouvelEncan.id) {
            if (Object.keys(lots.value).length === 0) {
                await store.dispatch('verifierEtatEncan')
            }
        }
    }, { immediate: true })

    // Forcer la réactivité
    watch(() => store.state.lots, () => {

    }, { deep: true });

    // Ajouter un watch sur les lots restants
    watch(lotsTriesParTemps, async (nouveauxLots) => {
        if (nouveauxLots.length === 0 && encan.value) {
            await store.dispatch('marquerToutesNotifLues')
        }
    }, { deep: true });
</script> 