<template>
    <div class="mt-1 container">
        <h2 class="fs-1 text-center fw-bold mt-5">Informations du membre</h2>
        <div class="row mt-5">
            <div class="col-md-4 col-12 mb-5 card card_avatar me-5">
                <div class="avatar-section mb-4 d-flex flex-column align-items-center">
                    <div class="avatar-container border border-1 rounded-circle mb-2">
                        <img :src="membre.avatar" alt="Avatar" class="img-fluid" />
                    </div>
                    <p class="nom_prenom">
                        {{ membre.generalInfo.nom }} {{ membre.generalInfo.prenom }}
                    </p>
                    <p class="pseudo">@{{ membre.generalInfo.pseudo }}</p>
                </div>
            </div>

            <div class="col-md-7">
                <div>
                    <form>
                        <!-- Informations générales -->
                        <div class="card mb-5">
                            <div class="card-header">Informations générales</div>
                            <div class="card-body">
                                <div class="row mt-2">
                                    <div class="col-md-4">
                                        <label for="nom">Nom</label>
                                        <input type="text"
                                               id="nom"
                                               class="form-control"
                                               disabled
                                               v-model="membre.generalInfo.nom" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="prenom">Prénom</label>
                                        <input type="text"
                                               id="prenom"
                                               disabled
                                               class="form-control"
                                               v-model="membre.generalInfo.prenom" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="pseudonyme">Pseudonyme</label>
                                        <input type="text"
                                               id="pseudonyme"
                                               disabled
                                               class="form-control"
                                               v-model="membre.generalInfo.pseudo" />
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-md-6">
                                        <label for="telephone">Téléphone</label>
                                        <div class="input-wrapper">
                                            <InputMask type="tel"
                                                       mask="(999) 999-9999"
                                                       disabled
                                                       id="telephone"
                                                       v-model="membre.generalInfo.telephone" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <label>Courriel</label>
                                        <input type="text"
                                               disabled
                                               class="form-control"
                                               v-model="membre.generalInfo.courriel" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Informations de carte de crédit -->
                        <div class="card my-5" v-if="membre.cartesCredit.length > 0">
                            <div class="card-header">Cartes de crédit</div>
                            <div class="card-body" v-for="carte in membre.cartesCredit">
                                <AffichageCarteCredit :carte="carte"></AffichageCarteCredit>
                            </div>
                        </div>

                        <!-- Informations d'adresse -->
                        <div class="card my-5">
                            <div class="card-header">Adresse</div>
                            <div class="card-body">
                                <div class="row mt-2">
                                    <div class="col-md-4">
                                        <label for="numeroCivique">Numéro civique</label>
                                        <input type="text"
                                               id="numeroCivique"
                                               disabled
                                               class="form-control"
                                               v-model="membre.adresse.numeroCivique" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="rue">Rue</label>
                                        <input type="text"
                                               class="form-control"
                                               disabled
                                               id="rue"
                                               v-model="membre.adresse.rue" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="appartement">Appartement</label>
                                        <input type="text"
                                               class="form-control"
                                               disabled
                                               id="appartement"
                                               v-model="membre.adresse.appartement" />
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-md-4">
                                        <label for="ville">Ville</label>
                                        <input type="text"
                                               class="form-control"
                                               disabled
                                               id="ville"
                                               v-model="membre.adresse.ville" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="province">Province</label>
                                        <input type="text"
                                               class="form-control"
                                               disabled
                                               id="province"
                                               v-model="membre.adresse.province" />
                                    </div>
                                    <div class="col-md-4">
                                        <label class="labels">Pays</label>
                                        <input type="text"
                                               class="form-control"
                                               disabled
                                               v-model="membre.adresse.pays" />
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-md-12">
                                        <label for="postalCode">Code postal</label>
                                        <div class="input-wrapper">
                                            <inputMask type="text"
                                                       id="postalCode"
                                                       class="form-control"
                                                       disabled
                                                       mask="a9a 9a9"
                                                       v-model="membre.adresse.codePostal" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
    import { ref, onMounted } from "vue";
    import { useStore } from "vuex";
    import InputMask from "primevue/inputmask";
    import AffichageCarteCredit from "@/components/views/AffichageCarteCredit.vue";
    import { useRoute } from "vue-router";

    const route = useRoute();
    const store = useStore();
    let membre = ref({
        generalInfo: {
            nom: "",
            prenom: "",
            courriel: "",
            telephone: "",
            pseudo: "",
            currentPassword: "",
            newPassword: "",
            confirmNewPassword: "",
        },
        cartesCredit: [],
        adresse: {
            numeroCivique: "",
            rue: "",
            appartement: "",
            ville: "",
            province: "",
            pays: "Canada",
            codePostal: "",
        },
        avatar: "",
    });

    onMounted(async () => {
        try {
            const membreId = route.params.id;
            const reponse = await store.dispatch("obtenirUnMembre", membreId);

            membre.value.generalInfo = {
                nom: reponse.name || "",
                prenom: reponse.firstName || "",
                courriel: reponse.email || "",
                telephone: reponse.telephone || "",
                pseudo: reponse.userName || "",
            };

            membre.value.cartesCredit = reponse.cartesCredit;

            membre.value.adresse = {
                numeroCivique: reponse.adresses[0].numeroCivique || "",
                rue: reponse.adresses[0].rue || "",
                appartement: reponse.adresses[0].appartement || "",
                ville: reponse.adresses[0].ville || "",
                province: mapProvince(reponse.adresses[0].province) || "",
                pays: reponse.adresses[0].pays || "",
                codePostal: reponse.adresses[0].codePostal || "",
            };
            membre.value.avatar = store.getters.avatarUrl || "";
        } catch (erreur) {
            console.error("Erreur lors de la récupération du membre");
        }
    });

    const mapProvince = (province) => {
        const mapProvince = new Map();
        mapProvince.set("QC", "Québec");
        mapProvince.set("ON", "Ontario");
        mapProvince.set("AB", "Alberta");
        mapProvince.set("BC", "Colombie-Britannique");
        mapProvince.set("MB", "Manitoba");
        mapProvince.set("NB", "Nouveau-Brunswick");
        mapProvince.set("NL", "Terre-Neuve-et-Labrador");
        mapProvince.set("NS", "Nouvelle-Écosse");
        mapProvince.set("PE", "Île-du-Prince-Édouard");
        mapProvince.set("SK", "Saskatchewan");
        mapProvince.set("NT", "Territoires du Nord-Ouest");
        mapProvince.set("NU", "Nunavut");
        mapProvince.set("YT", "Yukon");

        return mapProvince.get(province) || province;
    };
</script>

<style scoped>
    label {
        font-weight: 500;
        margin-left: 5px;
    }

    .input-wrapper .p-inputmask {
        width: 100%;
        border-radius: 0.35rem;
        border: 1px solid rgba(0, 0, 0, 0.12);
        padding: 0.375rem 0.75rem;
        background-color: rgba(240, 240, 240, 0.799);
        height: calc(1.5em + 0.75rem + 2px);
        transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    }

    input::placeholder {
        color: #6c757d;
        opacity: 1;
    }

    .p-inputmask input::placeholder {
        color: #6c757d;
        opacity: 1;
        font-weight: 400;
    }

    .input-wrapper .p-inputmask.is-invalid {
        border-color: #dc3545;
    }

    .input-wrapper .p-inputmask:focus {
        border-color: #80bdff;
        outline: 0;
        box-shadow: 0 0 0 0.2rem #80bdff;
    }

    .input-wrapper .p-inputmask:hover {
        border: 1px solid rgba(0, 0, 0, 0.12);
    }

    .card_avatar {
        height: 380px;
        padding-top: 20px;
    }

    .avatar-container img {
        height: 220px;
    }

    .nom_prenom {
        font-size: x-large;
        font-weight: 600;
        color: #052445;
    }

    .card {
        border-radius: 18px;
        box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.2), 0 3px 5px 0 rgba(0, 0, 0, 0.19);
    }
</style>
