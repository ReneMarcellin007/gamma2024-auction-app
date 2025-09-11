<template>
    <div class="d-flex flex-column align-items-center pb-3">
        <div class="text-center">
            <h1>Les Encans de Nantes</h1>

            <h2>Encans d'oeuvres d'art</h2>
        </div>

        <div class="row text-center w-100 justify-content-center">
            <p class="justifyText mb-0 col-11 col-md-6">
                Les Encans de Nantes au Québec est une entreprise spécialisée dans la gestion d'encans virtuels, axée principalement sur la vente d'œuvres d'art. Elle est co-propriétée par Frédérique Nantes, en collaboration avec deux autres associées, Monique Inverness et Patricia DeNice. L'entreprise est située à Stanstead, dans un bâtiment annexé à la bibliothèque Haskell, un lieu historique partagé entre le Canada et les États-Unis.
            </p>

            <router-link to="APropos" class="text-decoration-none text-black">
                <a>Lire plus ⮞</a>
            </router-link>
        </div>

        <div class="d-flex gap-5 flex-column flex-lg-row m-4 ">
            <AffichageEncanAccueil :type="passe" />
            <AffichageEncanAccueil :type="present" />
            <AffichageEncanAccueil :type="futur" />
        </div>

        <div class="col-11 col-md-6 p-3 p-md-4 mt-4 bleuMoyenFond text-center text-white contourDiv">

            <div class="fw-bold">
                <img src="/icons/MarteauBleuGris.png"
                     alt="Les Encans de Nantes"
                     height="60"
                     class="transpositionImage px-2 d-none d-lg-inline" />

                Déroulement d'un encan

                <img src="/icons/MarteauBleuGris.png"
                     alt="Les Encans de Nantes"
                     height="60"
                     class="px-2" />
            </div>

            <p class="justifyText mb-0">
                Les membres peuvent parcourir les lots d'œuvres d'art, faire des mises et des surenchères pendant la durée de l'encan, avec une soirée de clôture en direct. Pendant cette soirée, chaque lot est soumis à un intervalle de temps avant la fermeture définitive, avec la possibilité d'ajouter une minute en cas de surenchère. Un effet visuel indique les 20 dernières secondes pour chaque lot, et une notification par courriel est envoyée en cas de surenchère ou d'achat conclu.
            </p>
        </div>
    </div>
</template>

<script setup>
    import AffichageEncanAccueil from '@/components/views/AffichageEncanAccueil.vue'
    import { ref } from 'vue'

    const passe = ref(-1);
    const present = ref(0);
    const futur = ref(1);
</script>

<style scoped>
</style>