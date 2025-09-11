<template>
    <div class="container mt-5">
        <h1 class="text-center mb-4">Création d'un nouveau lot</h1>
        <form @submit.prevent="creerLot">
            <!-- Champ pour ajouter de nouvelles photos -->
            <div class="mb-3">
                <label for="nouvellesPhotos" class="form-label">
                    Ajouter des photos (maximum 5)
                </label>
                <input type="file"
                       class="form-control"
                       id="nouvellesPhotos"
                       multiple
                       @change="ajouterNouvellesPhotos"
                       accept="image/*">
            </div>
            <!-- Prévisualisation des nouvelles photos -->
            <div v-if="nouvellesPhotos.length > 0" class="mb-3">
                <h4>Nouvelles photos ({{ nouvellesPhotos.length }}/5)</h4>
                <div class="d-flex flex-wrap">
                    <div v-for="(photo, index) in nouvellesPhotos" :key="index" class="me-2 mb-2 position-relative">
                        <img :src="photo.preview"
                             alt="Nouvelle photo"
                             style="width: 100px; height: 100px; object-fit: cover;">
                        <button type="button"
                                class="btn btn-danger btn-sm position-absolute top-0 end-0"
                                @click="supprimerPhoto(index)">
                            X
                        </button>
                    </div>
                </div>
            </div>
            <!-- Message d'erreur pour les photos -->
            <div v-if="erreurPhotos" class="alert alert-danger mb-3">
                {{ erreurPhotos }}
            </div>
            <!-- Le reste des champs du formulaire -->
            <!--<div class="mb-3">
                <label for="code" class="form-label">Numéro</label>
                <input v-model="lot.numero" type="text" class="form-control" id="code" required>
            </div>-->
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
                <label for="idArtiste" class="form-label">Artiste</label>
                <select v-model="lot.idArtiste" class="form-select" id="idArtiste" required>
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
                <select v-model="lot.idEncan" class="form-select" id="idEncan" required>
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
            <button type="submit" class="btn btnSurvolerBleuMoyenFond text-white">Créer le lot</button>
        </form>
        <br>
        <div v-if="message" class="alert alert-success mt-3">
            {{ message }}
        </div>
        <div v-if="erreur && !erreurPhotos" class="alert alert-danger mt-3">
            {{ erreur }}
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import { useStore } from 'vuex';
    import { useRouter } from 'vue-router';

    const store = useStore();
    const router = useRouter();

    const lot = ref({
        numero: '',
        description: '',
        valeurEstimeMin: 0,
        valeurEstimeMax: 0,
        prixOuverture: 0,
        prixMinPourVente: 0,
        artiste: null,
        idCategorie: null,
        idVendeur: null,
        estLivrable: false,
        idMedium: null,
        idEncan: null,
        hauteur: 0,
        largeur: 0
    });
    const artistes = ref([]);
    const categories = ref([]);
    const vendeurs = ref([]);
    const mediums = ref([]);
    const encans = ref([]);
    const encansFuturs = ref([]);
    const nouvellesPhotos = ref([]);
    const message = ref('');
    const erreur = ref('');
    const erreurPhotos = ref('');

    onMounted(async () => {
        try {
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
        }
    });

    const ajouterNouvellesPhotos = (evenement) => {
        const fichiers = evenement.target.files;
        erreurPhotos.value = '';

        if (nouvellesPhotos.value.length + fichiers.length > 5) {
            erreurPhotos.value = "Vous ne pouvez pas ajouter plus de 5 photos par lot.";
            evenement.target.value = '';
            return;
        }

        for (let i = 0; i < fichiers.length; i++) {
            const fichier = fichiers[i];
            if (fichier.type.startsWith('image/')) {
                const preview = URL.createObjectURL(fichier);
                nouvellesPhotos.value.push({ fichier, preview });
            }
        }
    };

    const supprimerPhoto = (index) => {
        nouvellesPhotos.value.splice(index, 1);
    };

    const creerLot = async () => {
        try {
            const formData = new FormData();
            formData.append('Numero', "0");
            formData.append('Description', lot.value.description);
            formData.append('ValeurEstimeMin', lot.value.valeurEstimeMin);
            formData.append('ValeurEstimeMax', lot.value.valeurEstimeMax);
            formData.append('PrixOuverture', lot.value.prixOuverture);
            formData.append('PrixMinPourVente', lot.value.prixMinPourVente);
            formData.append('Artiste', lot.value.artiste);
            formData.append('IdCategorie', lot.value.idCategorie);
            formData.append('IdVendeur', lot.value.idVendeur);
            formData.append('EstLivrable', lot.value.estLivrable);
            formData.append('IdMedium', lot.value.idMedium);
            formData.append('IdEncan', lot.value.idEncan);
            formData.append('Hauteur', lot.value.hauteur);
            formData.append('Largeur', lot.value.largeur);

            nouvellesPhotos.value.forEach(({ fichier }) => {
                formData.append('Photos', fichier);
            });

            const reponse = await store.dispatch('creerLot', formData);
            if (reponse.success) {
                message.value = "Le lot a été créé avec succès.";
                erreur.value = '';
                setTimeout(() => {
                    router.push({ name: 'TableauDeBordInventaire' });
                }, 2000);
            }
        } catch (err) {
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
</script>
