<template>
    <div class="d-flex flex-column align-items-center pb-3">
        <h1>Encan {{ numeroEncan }}</h1>

        <div class="d-flex gap-2" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des encans...</span>
            </div>
            <p>Chargement de l'encan en cours...</p>
        </div>

        <div v-if="!chargement" class="w-100">
            <h5 class="text-center" v-if="encan == ''">
                Il n'y a présentement aucun encan ayant ce numéro
            </h5>

            <div v-else>
                <h5 class="text-center">{{ dateDebut }} - {{ dateFin }}</h5>
                <p class="text-center">
                    Date de début de la soirée de cloture: {{ soireeDate }}
                </p>

                <AffichageLots :idEncan="encan.id" />
            </div>
        </div>
    </div>
</template>

<script setup>
    import AffichageLots from "@/components/views/AffichageLots.vue";
    import { onMounted, ref } from "vue";
    import { useStore } from "vuex";

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

    const chargement = ref(true);
    const encan = ref("");
    const soireeDate = ref();
    const dateDebut = ref();
    const dateFin = ref();

    const props = defineProps({
        numeroEncan: String,
    });

    onMounted(async () => {
        const numero = props.numeroEncan;
        const reponse = await store.dispatch("chercherEncanParNumero", numero);
        encan.value = reponse.data;
        if (encan.value != "") {
            soireeDate.value = formatageDate(
                encan.value.dateDebutSoireeCloture,
                true,
                true
            );
            dateDebut.value = formatageDate(
                encan.value.dateDebut,
                affiche2Annees(),
                false
            );
            dateFin.value = formatageDate(encan.value.dateFin, true, false);

        }

        chargement.value = false;
    });

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

    function affiche2Annees() {
        let anneeFin = encan.value.dateFin.split("-")[0];
        let anneeDebut = encan.value.dateDebut.split("-")[0];

        if (anneeDebut == anneeFin) {
            return false;
        }
        return true;
    }
</script>

<style scoped></style>
