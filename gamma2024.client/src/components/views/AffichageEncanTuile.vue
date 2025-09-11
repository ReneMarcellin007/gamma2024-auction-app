<template>
    <div class="contourDiv p-2 align-items-center d-flex flex-column" role="button">
        <h2>Encan {{props.encan.numeroEncan}}</h2>
        <div class="d-flex w-100 mt-3 flex-row">
            <div class="col-12 d-flex align-items-center justify-content-center flex-column text-center">
                <div class="row justify-content-start w-100">
                    <p>{{dateDebut}} - {{dateFin}}</p>
                </div>
                <div class="row justify-content-start w-100">
                    <p>Soirée de clôture le {{soireeDate}}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref } from "vue";

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

    const props = defineProps({
        encan: Object,
    });
    const dateDebut = ref(
        formatageDate(props.encan.dateDebut, affiche2Annees(), false)
    );
    const dateFin = ref(formatageDate(props.encan.dateFin, true, false));
    const soireeDate = ref(
        formatageDate(props.encan.dateDebutSoireeCloture, affiche2Annees(), true)
    );

    function affiche2Annees() {
        let anneeFin = props.encan.dateFin.split("-")[0];
        let anneeDebut = props.encan.dateDebut.split("-")[0];

        if (anneeDebut == anneeFin) {
            return false;
        }
        return true;
    }

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