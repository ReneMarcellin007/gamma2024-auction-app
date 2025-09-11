<template>
    <section class="h-100">
        <div class="container py-5 h-100">
            <div class="row d-flex justify-content-center align-items-center h-100">
                <div class="col">
                    <form @submit.prevent="soumettreFormulaire">
                        <div class="card my-4">
                            <div class="row g-0">
                                <div class="col-xl-12">
                                    <div class="card-header">
                                        <h1 class="mb-5 mt-2 text-center">Création de vendeur</h1>
                                    </div>
                                    <div class="card-body p-md-3 text-black">
                                        <div v-if="message"
                                             :class="['alert', message.type === 'success' ? 'alert-success' : 'alert-danger', ]">
                                            {{ message.text }}
                                        </div>

                                        <div class="card">
                                            <div class="card-header fw-bold fs-5">
                                                Informations personnelles
                                            </div>
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-md-6 mb-">
                                                        <div class="form-outline">
                                                            <label class="form-label" for="prenom">Prénom</label>
                                                            <input v-model="vendeur.prenom"
                                                                   type="text"
                                                                   id="prenom"
                                                                   class="form-control form-control-lg"
                                                                   required />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-4">
                                                        <div class="form-outline">
                                                            <label class="form-label" for="nom">Nom</label>
                                                            <input v-model="vendeur.nom"
                                                                   type="text"
                                                                   id="nom"
                                                                   class="form-control form-control-lg"
                                                                   required />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-outline mb-4">
                                                            <label class="form-label" for="courriel">Courriel</label>
                                                            <input v-model="vendeur.courriel"
                                                                   type="email"
                                                                   id="courriel"
                                                                   class="form-control form-control-lg"
                                                                   required />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-outline mb-4 input-wrapper">
                                                            <label class="form-label" for="telephone">Téléphone</label>
                                                            <InputMask v-model="vendeur.telephone"
                                                                       mask="999-999-9999"
                                                                       type="tel"
                                                                       id="telephone"
                                                                       class="form-control form-control-lg"
                                                                       required />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card mt-4">
                                            <div class="card-header fw-bold fs-5">Adresses</div>
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-outline mb-4">
                                                            <label class="form-label" for="numeroCivique">Numéro civique</label>
                                                            <input v-model="vendeur.adresse.numeroCivique"
                                                                   type="text"
                                                                   id="numeroCivique"
                                                                   class="form-control form-control-lg"
                                                                   required />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-outline mb-4">
                                                            <label class="form-label" for="rue">Rue</label>
                                                            <input v-model="vendeur.adresse.rue"
                                                                   type="text"
                                                                   id="rue"
                                                                   class="form-control form-control-lg"
                                                                   required />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-outline mb-4">
                                                            <label class="form-label" for="appartement">Appartement (optionnel)</label>
                                                            <input v-model="vendeur.adresse.appartement"
                                                                   type="text"
                                                                   id="appartement"
                                                                   class="form-control form-control-lg" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-4">
                                                        <div class="form-outline">
                                                            <label class="form-label" for="ville">Ville</label>
                                                            <input v-model="vendeur.adresse.ville"
                                                                   type="text"
                                                                   id="ville"
                                                                   class="form-control form-control-lg"
                                                                   required />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-outline mb-4 input-wrapper">
                                                            <label class="form-label" for="codePostal">Code postal</label>
                                                            <InputMask v-model="vendeur.adresse.codePostal"
                                                                       mask="a9a 9a9"
                                                                       type="text"
                                                                       id="codePostal"
                                                                       class="form-control form-control-lg"
                                                                       required />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-4">
                                                        <div class="form-outline">
                                                            <label class="form-label" for="province">Province</label>
                                                            <select v-model="vendeur.adresse.province"
                                                                    id="province"
                                                                    class="form-select form-select-lg"
                                                                    required>
                                                                <option value="" disabled selected>
                                                                    Selectionner une province
                                                                </option>
                                                                <option v-for="province in provinces"
                                                                        :key="province"
                                                                        :value="province">
                                                                    {{ province }}
                                                                </option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-outline mb-4">
                                                    <label class="form-label" for="pays">Pays</label>
                                                    <input v-model="vendeur.adresse.pays"
                                                           type="text"
                                                           id="pays"
                                                           class="form-control form-control-lg"
                                                           required />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card-footer text-body-secondary">
                                            <div class="d-flex justify-content-end pt-3">
                                                <button type="button"
                                                        class="btn btn-light btn-lg"
                                                        @click="effacerFormulaire">
                                                    Réinitialiser
                                                </button>
                                                <button type="submit"
                                                        class="btn btnSurvolerBleuMoyenFond text-white btn-lg ms-2">
                                                    Créer le vendeur
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </section>
</template>

<script setup>
    import { ref } from "vue";
    import { useStore } from "vuex";
    import { useRouter } from "vue-router";
    import InputMask from "primevue/inputmask";

    const store = useStore();
    const router = useRouter();

    const provinces = [
        "Alberta",
        "Colombie-Britannique",
        "Île-du-Prince-Édouard",
        "Manitoba",
        "Nouveau-Brunswick",
        "Nouvelle-Écosse",
        "Ontario",
        "Québec",
        "Saskatchewan",
        "Terre-Neuve-et-Labrador",
        "Territoires du Nord-Ouest",
        "Nunavut",
        "Yukon",
    ];

    const vendeur = ref({
        nom: "",
        prenom: "",
        courriel: "",
        telephone: "",
        adresse: {
            numeroCivique: "",
            rue: "",
            ville: "",
            province: "",
            codePostal: "",
            pays: "",
            appartement: "",
        },
    });

    const message = ref(null);

    const soumettreFormulaire = async () => {
        try {
            const resultat = await store.dispatch("creerVendeur", vendeur.value);
            if (resultat.success) {
                message.value = { 
                    type: "success", 
                    text: resultat.message 
                };
                setTimeout(() => {
                    router.push("/affichagevendeurs");
                }, 2000);
            } else {
                message.value = {
                    type: "danger",
                    text: resultat.error
                };
            }
        } catch (erreur) {
            message.value = {
                type: "danger",
                text: typeof erreur === 'string' ? erreur : "Une erreur est survenue lors de la création du vendeur"
            };
        }
    };

    const effacerFormulaire = () => {
        vendeur.value = {
            nom: "",
            prenom: "",
            courriel: "",
            telephone: "",
            adresse: {
                numeroCivique: "",
                rue: "",
                ville: "",
                province: "",
                codePostal: "",
                pays: "",
                appartement: "",
            },
        };
    };
</script>

<style scoped>
    .card-registration .select-input.form-control[readonly]:not([disabled]) {
        font-size: 1rem;
        line-height: 2.15;
        padding-left: 0.75em;
        padding-right: 0.75em;
    }

    .select-arrow {
        top: 13px;
    }

    .form-select {
        appearance: none;
        background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='%23343a40' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M2 5l6 6 6-6'/%3e%3c/svg%3e");
        background-repeat: no-repeat;
        background-position: right 0.75rem center;
        background-size: 16px 12px;
        border: 1px solid #ced4da;
        border-radius: 0.25rem;
        padding: 0.375rem 2.25rem 0.375rem 0.75rem;
    }

        .form-select:focus {
            border-color: #86b7fe;
            outline: 0;
            box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25);
        }

    .input-wrapper .p-inputmask {
        width: 100%;
        border-radius: 0.25rem;
        padding: 0.375rem 0.75rem;
        background-color: white;
        border: 1px solid rgba(0, 0, 0, 0.15);
    }

        .input-wrapper .p-inputmask:hover {
            border: 1px solid rgba(0, 0, 0, 0.15);
        }

    .card {
        box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.2), 0 3px 5px 0 rgba(0, 0, 0, 0.19);
    }

    label {
        font-weight: 600;
        margin-left: 5px;
    }
</style>
