<template>
    <div v-if="chargement" class="d-flex justify-content-center">
        <div class="spinner-border" role="status">
            <span class="visually-hidden">Chargement</span>
        </div>
        <p>Chargement des details du lot...</p>
    </div>
    <div v-else>
        <div v-if="lot == ''" class="d-flex justify-content-center">
            <h1>Aucun lot ayant l'id {{props.idLot}}</h1>
        </div>
        <div class="container pb-3 d-flex flex-column align-items-center" v-else>
            <div class="row pb-3">
                <h1 class="text-center">Lot {{lot.numero}}</h1>
            </div>

            <div class="row w-100 align-items-stretch justify-content-around">
                <div class="col-12 col-lg-4 d-flex pt-3 bleuMoyenFond contourDiv text-white align-items-center mb-3 mb-lg-0">
                    <div class="col text-center">
                        <p>{{lot.artiste}}</p>
                        <p>{{lot.hauteur}} x {{lot.largeur}} po</p>
                        <p>{{lot.description}}</p>
                        <p>{{lot.medium}}</p>
                        <p>{{lot.categorie}}</p>
                        <p>Valeur estimée: {{(lot.valeurEstimeMin).toFixed(0)}}$ à {{(lot.valeurEstimeMax).toFixed(0)}}$</p>

                        <img v-if="lot.estLivrable" src="/icons/Livrable.png" alt="livrable" height="80" />
                        <img v-else src="/icons/NonLivrable.png" alt="livrable" height="80" />
                    </div>
                </div>
                <div class="col-12 col-lg-7 d-flex flex-column justify-content-between">
                    <div id="carouselExampleIndicators" class="carousel">

                        <div class="carousel-inner">

                            <div v-for="(image, index) in lot.photos">
                                <div :class="{active: index==0, 'carousel-item': true}">
                                    <div class="d-flex justify-content-center">
                                        <img :src="urlApi + image" alt="..." class="w-100">
                                    </div>
                                </div>
                            </div>

                        </div>

                    </div>

                    <div class="d-flex justify-content-between align-items-center mt-2 carouselFondIndicators p-1">

                        <a class="opacity-100 d-flex align-items-center text-decoration-none" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev">
                            <span class="text-black fs-2" aria-hidden="true">⮜</span>
                            <span class="visually-hidden">Previous</span>
                        </a>

                        <div class="gap-1 d-flex flex-wrap justify-content-around">

                            <div v-for="(image, index) in lot.photos">
                                <a data-bs-target="#carouselExampleIndicators" :data-bs-slide-to="index" class="active " aria-current="true" :aria-label="'Slide ' + (index+1)" role="button">
                                    <img :src="urlApi + image" height="60" alt="Image du lot">
                                </a>
                            </div>
                        </div>

                        <a class="opacity-100 d-flex align-items-center text-decoration-none" data-bs-target="#carouselExampleIndicators" data-bs-slide="next">
                            <span class="text-black fs-2" aria-hidden="true">⮞</span>
                            <span class="visually-hidden">Next</span>
                        </a>
                    </div>
                </div>
            </div>
        </div>

    </div>
</template>

<script setup>
    import { onMounted, ref } from "vue";
    import { useStore } from "vuex";

    const store = useStore();

    const chargement = ref(true);
    const urlApi = ref("/api")
    const lot = ref('');

    const props = defineProps({
        idLot: String,
    });

    onMounted(async () => {
        const id = props.idLot;
        const reponse = await store.dispatch("chercherDetailsLotParId", id);
        lot.value = reponse.data
        urlApi.value = await store.state.api.defaults.baseURL.replace("\api", "")

        chargement.value = false;
    });
</script>

<style scoped>
    .carouselFondIndicators {
        background-color: #C3CBD8;
    }
</style>