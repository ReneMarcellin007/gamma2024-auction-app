<template>
    <div class="d-flex flex-column align-items-center pb-3">
        <div class="d-flex gap-2" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des encans...</span>
            </div>
            <p>Chargement de l'encan en cours...</p>
        </div>

        <div v-if="!chargement" class="w-100">
            <div v-if="type === 'aucun'">
                <h5 class="text-center">
                    Il n'y a présentement aucun encan en cours
                </h5>
            </div>

            <div v-else-if="type === 'courant'">
                <h1 class="text-center">
                    Encan en cours <span v-if="encan != ''">({{ encan.numeroEncan }})</span>
                </h1>
                <p class="text-center">
                    Date de début de la soirée de clotûre: {{ soireeDate }}
                </p>

                <p v-if="tempsRestant" class="text-center mb-5 fs-1 fw-bolder textCount">
                    Début de la soirée de clotûre dans
                    <span class="fw-bold" v-if="tempsRestant.days > 0">{{ tempsRestant.days }} jour{{ tempsRestant.days > 1 ? 's' : '' }}</span>
                    {{ tempsRestant.hours }}h
                    {{ tempsRestant.minutes }}m
                    {{ tempsRestant.seconds }}s
                </p>

                <AffichageLots :idEncan="encan.id" />
            </div>

            <div v-else-if="type === 'soireeCloture'">
                <SoireeCloture :encan="encan" />
            </div>
        </div>
    </div>
</template>

<script setup>
    import AffichageLots from "@/components/views/AffichageLots.vue";
    import SoireeCloture from "@/components/views/SoireeCloture.vue";
    import { onMounted, ref, onUnmounted, watch } from "vue";
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";

    let mois = [
        "janvier",
        "février",
        "mars",
        "avril",
        "mai",
        "juin",
        "juillet",
        "août",
        "septembre",
        "octobre",
        "novembre",
        "décembre",
    ];

    const store = useStore();
    const router = useRouter();
    const chargement = ref(true);
    const encan = ref("");
    const soireeDate = ref();
    const type = ref(null);

    // Vérification périodique de l'état
    const intervalle = ref(null);

    const tempsRestant = ref(null);
    let intervalleDeDecompte = null;

    const remettreAJourDecompte = () => {
        if (!encan.value?.dateDebutSoireeCloture) return;

        const now = new Date();
        const targetDate = new Date(encan.value.dateDebutSoireeCloture);
        let difference = targetDate - now;

        if (difference <= 0) {
            tempsRestant.value = {
                days: 0,
                hours: 0,
                minutes: 0,
                seconds: 0
            };
            if (intervalleDeDecompte) {
                clearInterval(intervalleDeDecompte);
            }
            return;
        }

        const secondsInMinute = 60;
        const secondsInHour = 60 * secondsInMinute;
        const secondsInDay = 24 * secondsInHour;

        const days = Math.floor(difference / (secondsInDay * 1000));
        difference %= secondsInDay * 1000;

        const hours = Math.floor(difference / (secondsInHour * 1000));
        difference %= secondsInHour * 1000;

        const minutes = Math.floor(difference / (secondsInMinute * 1000));
        difference %= secondsInMinute * 1000;

        const seconds = Math.floor(difference / 1000);

        tempsRestant.value = { days, hours, minutes, seconds };
    };

    const verifierEtat = async () => {
        try {
            const maintenant = new Date();
            const etatType = await store.dispatch('verifierEtatEncan');
            type.value = etatType;
            encan.value = store.state.encanCourant;

            // Vérifier si encan.value existe avant d'accéder à ses propriétés
            if (encan.value && encan.value.dateDebutSoireeCloture) {
                soireeDate.value = formatageDate(
                    encan.value.dateDebutSoireeCloture,
                    true,
                    true
                );

                // Vérifier si on est exactement à l'heure de début
                const debutSoiree = new Date(encan.value.dateDebutSoireeCloture);
                const diffMs = debutSoiree - maintenant;

                if (diffMs > 0 && diffMs < 1000) { // Si moins d'une seconde avant le début
                    // Programmer la transition exacte
                    setTimeout(async () => {
                        await router.replace({ name: 'EncanPresent' });
                    }, diffMs);
                }
            }
        } catch (error) {
            console.error("Erreur lors de la vérification de l'état:", error);
        }
    };

    onMounted(async () => {
        await verifierEtat();
        // Démarrer la surveillance des transitions
        await store.dispatch('surveillerTransitionEncan')
        intervalle.value = setInterval(verifierEtat, 2000);

        // Ajouter le countdown
        remettreAJourDecompte();
        intervalleDeDecompte = setInterval(remettreAJourDecompte, 1000);

        chargement.value = false;
    });

    onUnmounted(() => {
        if (intervalle.value) {
            clearInterval(intervalle.value);
        }
        if (intervalleDeDecompte) {
            clearInterval(intervalleDeDecompte);
        }
    });

    watch(
        () => store.state.typeEncanCourant,
        async (newType, oldType) => {
            if (oldType === 'courant' && newType === 'soireeCloture') {
                await router.replace({ name: 'EncanPresent' });
            }
        },
        { immediate: true }
    );

    function formatageDate(dateTexte, siAnnee, siHeure) {
        let sep = dateTexte.split("T");
        let dates = sep[0].split("-");
        let temps = sep[1].split(":");

        let resultat = dates[2] + " " + mois[dates[1] - 1];

        if (siAnnee) {
            resultat += " " + dates[0];
        }
        if (siHeure) {
            resultat += " à " + parseInt(temps[0]) + "h" + temps[1];
        }
        return resultat;
    }
</script>


