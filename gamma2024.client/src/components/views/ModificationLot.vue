<template>
    <div class="container mt-5">
        <h1 class="text-center mb-4">Modification du lot</h1>
        <form @submit.prevent="modifierLot" v-if="lot">
            <!-- Bloc des photos existantes -->
            <div v-if="lot.photosModifie && lot.photosModifie.length > 0" class="mb-3">
                <h4>Photos existantes</h4>
                <div class="d-flex flex-wrap">
                    <div v-for="photo in lot.photosModifie" :key="photo.id" class="me-2 mb-2 position-relative">
                        <img :src="chercherImageUrl(photo.url)" alt="Photo du lot"
                             style="width: 100px; height: 100px; object-fit: cover; border: 1px solid #ddd;">
                        <button @click.prevent="marquerPhotoASupprimer(photo.id)" class="btn btn-danger btn-sm position-absolute top-0 end-0">X</button>
                    </div>
                </div>
            </div>
            <div v-if="photosASupprimer.length > 0" class="alert alert-warning">
                {{ photosASupprimer.length }} photo(s) marquée(s) pour suppression
            </div>
            <!-- Champ pour ajouter de nouvelles photos -->
            <div class="mb-3">
                <label for="nouvellesPhotos" class="form-label">Ajouter des photos</label>
                <input type="file"
                       class="form-control"
                       id="nouvellesPhotos"
                       multiple
                       @change="ajouterNouvellesPhotos"
                       :disabled="!peutAjouterPhotos">
                <small class="text-muted">
                    {{ messagePhotosRestantes }}
                </small>
            </div>
            <!-- Prévisualisation des nouvelles photos -->
            <div v-if="nouvellesPhotos.length > 0" class="mb-3">
                <h4>Nouvelles photos</h4>
                <div class="d-flex flex-wrap">
                    <div v-for="(photo, index) in nouvellesPhotos" :key="index" class="me-2 mb-2 position-relative">
                        <img :src="photo.preview" alt="Nouvelle photo" style="width: 100px; height: 100px; object-fit: cover;">
                        <button @click.prevent="supprimerNouvellePhoto(index)" class="btn btn-danger btn-sm position-absolute top-0 end-0">X</button>
                    </div>
                </div>
            </div>
            <!-- Le reste des champs du formulaire -->
            <div class="mb-3">
                <label for="code" class="form-label">Numéro</label>
                <input v-model="lot.numero" type="text" class="form-control" disabled id="code" required>
            </div>
            <div class="mb-3">
                <label for="description" class="form-label">Description</label>
                <textarea v-model="lot.description" class="form-control" id="description" required></textarea>
            </div>
            <div class="mb-3">
                <label for="valeurEstimeMin" class="form-label">Valeur estimée minimale</label>
                <input v-model.number="lot.valeurEstimeMin" type="number" class="form-control" id="valeurEstimeMin" required>
            </div>
            <div class="mb-3">
                <label for="valeurEstimeMax" class="form-label">Valeur estimée maximale</label>
                <input v-model.number="lot.valeurEstimeMax" type="number" class="form-control" id="valeurEstimeMax" required>
            </div>
            <div class="mb-3">
                <label for="prixOuverture" class="form-label">Prix d'ouverture</label>
                <input v-model.number="lot.prixOuverture" type="number" class="form-control" id="prixOuverture" required>
            </div>
            <div class="mb-3">
                <label for="prixMinPourVente" class="form-label">Prix minimum pour vente</label>
                <input v-model.number="lot.prixMinPourVente" type="number" class="form-control" id="prixMinPourVente" required>
            </div>
            <div class="mb-3">
                <label for="artiste" class="form-label">Artiste</label>
                <select v-model="lot.artiste" class="form-select" id="artiste" required>
                    <option v-for="artiste in artistes" :key="artiste.nomArtiste" :value="artiste.nomArtiste">
                        {{ artiste.nomArtiste }}
                    </option>
                </select>
            </div>
            <div class="mb-3">
                <label for="idCategorie" class="form-label">Catégorie</label>
                <select v-model="lot.idCategorie" class="form-select" id="idCategorie" required>
                    <option v-for="categorie in categories" :key="categorie.id" :value="categorie.id">
                        {{ categorie.nom }}
                    </option>
                </select>
            </div>
            <div class="mb-3">
                <label for="idVendeur" class="form-label">Vendeur</label>
                <select v-model="lot.idVendeur" class="form-select" id="idVendeur" required>
                    <option v-for="vendeur in vendeurs" :key="vendeur.id" :value="vendeur.id">
                        {{ vendeur.prenom }} {{ vendeur.nom }}
                    </option>
                </select>
            </div>
            <div class="mb-3 form-check">
                <input v-model="lot.estLivrable" type="checkbox" class="form-check-input" id="estLivrable">
                <label class="form-check-label" for="estLivrable">Est livrable</label>
            </div>
            <div class="mb-3">
                <label for="idMedium" class="form-label">Medium</label>
                <select v-model="lot.idMedium" class="form-select" id="idMedium" required>
                    <option v-for="medium in mediums" :key="medium.id" :value="medium.id">
                        {{ medium.type }}
                    </option>
                </select>
            </div>
            <div class="mb-3">
                <label for="idEncan" class="form-label">Encan</label>
                <select v-model="lot.idEncanModifie" class="form-select" id="idEncan" required>
                    <option v-for="encan in encansFuturs" :key="encan.id" :value="encan.id">
                        {{ encan.numeroEncan }}
                    </option>
                </select>
            </div>
            <div class="mb-3">
                <label for="hauteur" class="form-label">Hauteur</label>
                <input v-model.number="lot.hauteur" type="number" class="form-control" id="hauteur" required>
            </div>
            <div class="mb-3">
                <label for="largeur" class="form-label">Largeur</label>
                <input v-model.number="lot.largeur" type="number" class="form-control" id="largeur" required>
            </div>
            <button type="submit" class="btn btnSurvolerBleuMoyenFond text-white">Modifier le lot</button>
        </form>
        <br>
        <div v-if="message" class="alert alert-success mt-3">
            {{ message }}
        </div>
        <div v-if="erreur" class="alert alert-danger mt-3">
            {{ erreur }}
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted, onBeforeUnmount, computed } from 'vue';
    import { useStore } from 'vuex';
    import { useRouter, useRoute } from 'vue-router';

    const store = useStore();
    const router = useRouter();
    const route = useRoute();

    const lot = ref(null);
    const artistes = ref([]);
    const categories = ref([]);
    const vendeurs = ref([]);
    const mediums = ref([]);
    const encans = ref([]);
    const encansFuturs = ref([]);
    const nouvellesPhotos = ref([]);
    const photosASupprimer = ref([]);
    const message = ref('');
    const erreur = ref('');

    const nombreDePhotosMaximal = 5;

    // Calculer le nombre total de photos actuelles et futures
    const nombreTotalPhotos = computed(() => {
        const photosExistantesNonSupprimees = (lot.value?.photosModifie?.length || 0);
        const nombreNouvellesPhotos = nouvellesPhotos.value.length;
        return photosExistantesNonSupprimees + nombreNouvellesPhotos;
    });

    // Calculer le nombre de photos disponibles
    const photosDisponibles = computed(() => {
        return nombreDePhotosMaximal - nombreTotalPhotos.value;
    });

    // Vérifier si on peut ajouter plus de photos
    const peutAjouterPhotos = computed(() => {
        return photosDisponibles.value > 0;
    });

    // Message pour indiquer combien de photos peuvent encore être ajoutées
    const messagePhotosRestantes = computed(() => {
        if (photosDisponibles.value <= 0) {
            return "Nombre maximum de photos atteint (5 photos maximum)";
        }
        return `Vous pouvez encore ajouter ${photosDisponibles.value} photo${photosDisponibles.value > 1 ? 's' : ''}`;
    });

    // Fonction pour supprimer une nouvelle photo
    const supprimerNouvellePhoto = (index) => {
        URL.revokeObjectURL(nouvellesPhotos.value[index].preview);
        nouvellesPhotos.value.splice(index, 1);
    };

    onMounted(async () => {
        try {
            lot.value = await store.dispatch('obtenirLot', route.params.id);
            artistes.value = await store.dispatch("obtenirArtistes");
            categories.value = await store.dispatch('obtenirCategories');
            vendeurs.value = await store.dispatch('obtenirVendeurs');
            mediums.value = await store.dispatch('obtenirMediums');
            encans.value = await store.dispatch('obtenirEncans');

            const dernierEncan = await store.dispatch(
                "chercherNumeroEncanEnCours"
            );
            if (dernierEncan.data != 0) {
                var numeroLimiteEncan = dernierEncan.data;
            }
            else {
                const encansPasses = await store.dispatch(
                    "chercherEncansPasses"
                );
                var numeroLimiteEncan = Math.max(...encansPasses.data.map(encan => encan.numeroEncan))
            }
            encansFuturs.value = encans.value.filter((encan) => encan.numeroEncan > numeroLimiteEncan);

        } catch (erreur) {
            console.error("Erreur lors de la récupération des données:", erreur);
            // Optionnel: Afficher un message d'erreur à l'utilisateur
            message.value = {
                type: 'error',
                text: "Erreur lors du chargement des données. Veuillez réessayer."
            };
        }
    });

    onBeforeUnmount(() => {
        nouvellesPhotos.value.forEach(photo => {
            URL.revokeObjectURL(photo.preview);
        });
    });

    const marquerPhotoASupprimer = (photoId) => {
        if (!photosASupprimer.value.includes(photoId)) {
            photosASupprimer.value.push(photoId);
            lot.value.photosModifie = lot.value.photosModifie.filter(photo => photo.id !== photoId);
        }
    };

    const modifierLot = async () => {
        try {
            const formData = new FormData();

            // Ajoutez chaque champ au FormData
            formData.append('Id', lot.value.id);
            formData.append('Numero', lot.value.numero || '');
            formData.append('Description', lot.value.description || '');
            formData.append('ValeurEstimeMin', lot.value.valeurEstimeMin || 0);
            formData.append('ValeurEstimeMax', lot.value.valeurEstimeMax || 0);
            formData.append('PrixOuverture', lot.value.prixOuverture || 0);
            formData.append('PrixMinPourVente', lot.value.prixMinPourVente || 0);
            formData.append('Artiste', lot.value.artiste || '');
            formData.append('IdCategorie', lot.value.idCategorie || 0);
            formData.append('IdVendeur', lot.value.idVendeur || 0);
            formData.append('EstLivrable', lot.value.estLivrable || false);
            formData.append('IdMedium', lot.value.idMedium || 0);
            formData.append('IdEncanModifie', lot.value.idEncanModifie || 0);
            formData.append('Hauteur', lot.value.hauteur || 0);
            formData.append('Largeur', lot.value.largeur || 0);

            // Ajoutez les nouvelles photos
            nouvellesPhotos.value.forEach(({ file }) => {
                formData.append('NouvellesPhotos', file);
            });

            // Ajoutez les IDs des photos à supprimer
            photosASupprimer.value.forEach((photoId) => {
                formData.append('PhotosASupprimer[]', photoId);
            });

            const reponse = await store.dispatch('modifierLot', { id: route.params.id, lotData: formData });
            if (reponse.success) {
                message.value = "Le lot a été modifié avec succès.";
                erreur.value = '';
                setTimeout(() => {
                    router.push({ name: 'TableauDeBordInventaire' });
                }, 2000);
            }
        } catch (err) {
            // Correction de la gestion d'erreur
            if (err.response?.data) {
                erreur.value = err.response.data;
            } else if (typeof err === 'string') {
                erreur.value = err;
            } else {
                erreur.value = "Une erreur est survenue lors de la création du lot";
            }
            message.value = '';
        }
    };

    const chercherImageUrl = computed(() => (url) => {
        if (!url) return '';
        const baseUrl = store.state.api.defaults.baseURL.replace('\api', '');
        return baseUrl + url;
    });

    const ajouterNouvellesPhotos = (evenement) => {
        const fichiers = evenement.target.files;
        const photosRestantes = photosDisponibles.value;

        if (photosRestantes <= 0) {
            erreur.value = "Nombre maximum de photos atteint";
            evenement.target.value = ''; // Réinitialiser l'input
            return;
        }

        // Limiter le nombre de nouvelles photos à ajouter
        const nombrePhotosAAjouter = Math.min(fichiers.length, photosRestantes);

        for (let i = 0; i < nombrePhotosAAjouter; i++) {
            const fichier = fichiers[i];
            if (fichier.type.startsWith('image/')) {
                const preview = URL.createObjectURL(fichier);
                nouvellesPhotos.value.push({ fichier, preview });
            }
        }

        // Afficher un message si certaines photos n'ont pas été ajoutées
        if (fichiers.length > photosRestantes) {
            message.value = `Seules ${nombrePhotosAAjouter} photo(s) ont été ajoutées pour respecter la limite de 5 photos au total`;
        }

        // Réinitialiser l'input
        evenement.target.value = '';
    };
</script>

<style scoped>
    .position-relative {
        position: relative;
    }

    .btn-danger.btn-sm {
        padding: 0.25rem 0.5rem;
        font-size: 0.875rem;
        line-height: 1.5;
        border-radius: 0.2rem;
    }
</style>

