<template>
    <div class="container mt-4">
        <h2 class="text-center mb-4">Historique des mises</h2>

        <div class="d-flex gap-2 w-100 justify-content-center" v-if="chargement">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Chargement des mises...</span>
            </div>
            <p>Chargement des mises en cours...</p>
        </div>

        <div v-else>
            <div v-if="!misesParEncan || misesParEncan.length === 0" class="text-center">
                <h5>Aucune mise trouvée</h5>
            </div>

            <div v-else class="d-flex flex-column gap-4">
                <!-- Contrôles d'affichage -->
                <div class="d-flex flex-row-reverse px-4 me-2 gap-2">
                    <button class="rounded btn btnSurvolerBleuMoyenFond"
                            @click="toggleAffichage">
                        <img :src="siTuile ? '/icons/ListeOption.png' : '/icons/TableauOption.png'"
                             :alt="siTuile ? 'Affichage en liste' : 'Affichage en tableau'"
                             height="25" />
                    </button>
                </div>

                <!-- Section par encan -->
                <div v-for="encan in misesParEncan" 
                     :key="encan.numeroEncan" 
                     class="mb-5">
                    <h3 class="mb-4">Encan {{ encan.numeroEncan }}</h3>
                    
                    <div v-if="siTuile" 
                         class="row row-cols-lg-5 row-cols-md-3 row-cols-sm-2 row-cols-1">
                        <div v-for="lot in encan.lots" 
                             :key="lot.lotId" 
                             class="col p-2">
                            <LotTuile :lotRecu="mapLotToViewModel(lot)" />
                        </div>
                    </div>
                    
                    <div v-else class="d-flex flex-column gap-3">
                        <div v-for="lot in encan.lots" 
                             :key="lot.lotId" 
                             class="w-100">
                            <LotListe :lotRecu="mapLotToViewModel(lot)" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useStore } from 'vuex';
import LotTuile from './LotTuile.vue';
import LotListe from './LotListe.vue';

const store = useStore();
const chargement = ref(true);
const siTuile = ref(true);
const misesParEncan = ref([]);

const toggleAffichage = () => {
    siTuile.value = !siTuile.value;
};

const mapLotToViewModel = (lot) => {
    // Nettoyer le chemin de l'image
    const cleanImagePath = lot.photoPrincipale
        ?.replace(/\\/g, '/') // Remplacer les backslashes par des forward slashes
        ?.replace(/^Images\//, ''); // Enlever le "Images/" initial si présent

    return {
        id: lot.lotId,
        numero: lot.numero,
        artiste: lot.artiste,
        hauteur: lot.hauteur,
        largeur: lot.largeur,
        mise: lot.derniereMise,
        valeurEstimeMin: lot.valeurEstimeMin,
        valeurEstimeMax: lot.valeurEstimeMax,
        prixOuverture: lot.prixOuverture,
        prixMinPourVente: lot.prixMinPourVente,
        estVendu: lot.estVendu,
        photos: [{
            lien: cleanImagePath ? `/Images/${cleanImagePath}` : ''
        }],
        dateFinDecompteLot: lot.dateFinDecompteLot,
        userHasBid: true,
        estPlusHautEncherisseur: lot.estPlusHautEncherisseur,
        estLivrable: lot.estLivrable,
        description: lot.description,
        idCategorie: lot.idCategorie,
        categorie: lot.categorie,
        idMedium: lot.idMedium,
        medium: lot.medium,
        idVendeur: lot.idVendeur,
        vendeur: lot.vendeur,
        seraLivree: lot.seraLivree
    };
};

const chargerMises = async () => {
    try {
        chargement.value = true;
        const response = await store.dispatch('getUserBidsGroupedByEncan');
        misesParEncan.value = response || [];
    } catch (error) {
        console.error('Erreur lors du chargement des mises:', error);
        misesParEncan.value = [];
    } finally {
        chargement.value = false;
    }
};

onMounted(() => {
    chargerMises();
});
</script>

<style scoped>
    /* Réutiliser les styles d'AffichageLots si nécessaire */
</style> 