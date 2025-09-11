<template>
    <router-link class="text-decoration-none card d-flex align-self-stretch"
                 :class="{
      'user-bid': estUtilisateurPlusHauteMise,
      'user-outbid': estUtilisateurMiseDepassee
    }"
                 :to="{ name: 'DetailsLot', params: { idLot: lot.id } }">
        <div class="align-self-stretch h-100">
            <div class="card-body d-flex flex-column justify-content-between h-100">
                <div class="d-flex align-items-middle justify-content-between">
                    <p class="fs-8 pe-2 mt-1 mb-0 text-truncate">
                        Lot {{ lot.numero }} - {{ lot.artiste }} {{ lot.hauteur }}x{{ lot.largeur }}po
                    </p>

                    <img src="/icons/Livrable.png"
                         height="25"
                         alt="Livrable"
                         v-if="lot.estLivrable" />

                    <img src="/icons/NonLivrable.png"
                         height="25"
                         alt="Non livrable"
                         v-else />
                </div>

                <div class="row justify-content-around align-items-middle">
                    <img :src="urlImage"
                         class="img-fluid my-2 col-8 col-sm-10 col-md-12"
                         alt="Image du lot" />
                </div>

                <p class="text-center mb-0 fs-9">
                    Mise actuelle: {{ formatMontant(miseActuelle) }}$
                </p>
                <p class="text-center mb-0 fs-9">
                    Valeur: {{ formatMontant(lot.valeurEstimeMin) }}$ -
                    {{ formatMontant(lot.valeurEstimeMax) }}$
                </p>
                <p class="text-center mb-0 text-muted fs-9">
                    {{ nombreOffres }} {{ nombreOffres <= 1 ? 'offre' : 'offres' }}
                </p>
                <p v-if="showDecompte" class="text-center mb-0" :class="{'text-danger': tempsRestant < 60}">
                    {{ formatDecompte }}
                </p>

                <div class="d-flex justify-content-around pt-2">
                    <button type="button"
                            class="btn text-white btnSurvolerBleuMoyenFond fs-9"
                            @click.stop.prevent="gererCliquerSurMiser"
                            v-if="!estAdmin && peutMiser">
                        Miser {{ formatMontant(chercherMiseMinimale) }}$
                    </button>
                </div>
            </div>
        </div>
    </router-link>

    <ModalMise ref="modalMise"
               :lot="lotPourModal"
               @miseConfirmee="onMiseConfirmee" />
</template>

<script setup>
    import { onMounted, ref, computed, watch, onUnmounted, nextTick } from "vue";
    import { useStore } from "vuex";
    import ModalMise from '@/components/modals/ModalMise.vue';
    import { toast } from 'vue3-toastify';
    import { useRouter } from 'vue-router';

    const store = useStore();
    const router = useRouter();
    const props = defineProps({
        lotRecu: Object,
        showDecompte: {
            type: Boolean,
            default: false
        }
    });

    const urlApi = ref("/api");
    const userLastBid = ref(0);
    const modalMise = ref(null);

    const calculerPasEnchere = (montant) => {
        if (montant <= 199.0) return 10.0;
        if (montant <= 499.0) return 25.0;
        if (montant <= 999.0) return 50.0;
        if (montant <= 1999.0) return 100.0;
        if (montant <= 4999.0) return 200.0;
        if (montant <= 9999.0) return 250.0;
        if (montant <= 19999.0) return 500.0;
        if (montant <= 49999.0) return 1000.0;
        if (montant <= 99999.0) return 2000.0;
        if (montant <= 499999.0) return 5000.0;
        return 10000.0;
    };

    const formatMontant = (montant) => {
        const valeur = Number(montant);
        return isNaN(valeur) ? '0' : Math.floor(valeur).toString();
    };

    const miseActuelle = computed(() => {
        const lot = store.getters.getLot(props.lotRecu.id);
        if (lot?.mise !== undefined) {
            return lot.mise;
        }
        return props.lotRecu.mise || 0;
    });

    const chercherMiseMinimale = computed(() => {
        if (!miseActuelle.value || miseActuelle.value === 0) {
            return props.lotRecu?.prixOuverture || 0;
        }
        return calculerPasEnchere(miseActuelle.value) + miseActuelle.value;
    });

    // Garder lot comme ref pour le template
    const lot = ref({
        id: 0,
        numero: "",
        description: "",
        valeurEstimeMin: 0,
        valeurEstimeMax: 0,
        artiste: " ",
        mise: 0,
        estVendu: true,
        dateFinVente: "",
        estLivrable: true,
        largeur: 0,
        hauteur: 0,
        photos: [
            {
                id: 0,
                lien: "",
                idLot: 0,
                lot: null,
            },
        ],
    });

    // Créer un objet computed pour le lot à passer au modal
    const lotPourModal = computed(() => {
        // Récupérer le lot à jour depuis le store
        const lotActuel = store.getters.getLot(props.lotRecu.id);
        return {
            id: props.lotRecu.id,
            numero: props.lotRecu.numero,
            mise: lotActuel?.mise || props.lotRecu.mise || 0,
            prixOuverture: props.lotRecu.prixOuverture,
            prixMinPourVente: props.lotRecu.prixMinPourVente,
            description: props.lotRecu.description,
            valeurEstimeMin: props.lotRecu.valeurEstimeMin,
            valeurEstimeMax: props.lotRecu.valeurEstimeMax,
            artiste: props.lotRecu.artiste,
            estVendu: props.lotRecu.estVendu,
            dateFinVente: props.lotRecu.dateFinVente,
            estLivrable: props.lotRecu.estLivrable,
            largeur: props.lotRecu.largeur,
            hauteur: props.lotRecu.hauteur,
            photos: props.lotRecu.photos
        };
    });

    const estUtilisateurPlusHauteMise = computed(() => {
        const lotActuel = store.getters.getLot(props.lotRecu.id);
        const userId = store.state.user?.id;
        return store.getters.hasUserBidOnLot(props.lotRecu.id) &&
            lotActuel?.idClientMise === userId;
    });

    const estMontantValide = (montant) => {
        return montant !== undefined && montant !== null && !isNaN(Number(montant));
    };

    const urlImage = computed(() => {
        const lotStore = store.getters.getLot(props.lotRecu.id);
        if (lotStore?.photos?.[0]?.lien) {
            return urlApi.value + lotStore.photos[0].lien;
        }
        return urlApi.value + props.lotRecu.photos[0].lien;
    });

    const estUtilisateurMiseDepassee = computed(() => {
        const lotActuel = store.getters.getLot(props.lotRecu.id);
        const aFaitUneMise = store.getters.hasUserBidOnLot(props.lotRecu.id) || userLastBid.value > 0;
        const nestPasPlusHautEncherisseur = lotActuel?.idClientMise !== store.state.user?.id;
        const miseExiste = lotActuel?.mise > 0;
        const estSurenchere = lotActuel?.mise > userLastBid.value;

        return aFaitUneMise && nestPasPlusHautEncherisseur && miseExiste && estSurenchere;
    });

    const gererNouvelleEnchere = async (data) => {

        if (data.idLot === props.lotRecu.id) {
            store.commit('updateLotMise', {
                idLot: data.idLot,
                montant: data.montant,
                userId: data.userId,
                userLastBid: data.userLastBid,
                nombreMises: data.nombreMises
            });

            if (data.userLastBid && data.userLastBid.userId === store.state.user?.id) {
                userLastBid.value = data.userLastBid.montant;
            }

            nextTick(() => {
            });
        }
    };

    const ouvrirModalMise = (evenement) => {
        evenement.stopPropagation();
        // Forcer une mise à jour du lot depuis le store
        const lotActuel = store.getters.getLot(props.lotRecu.id);
        if (lotActuel) {
            lot.value = { ...lotActuel };
        }
        modalMise.value.presenterModal();
    };

    const onMiseConfirmee = async (montant) => {
        lot.value.mise = montant;

        try {
            const reponse = await store.dispatch('chercherDetailsLotParId', lot.value.id);
            if (reponse && reponse.data) {
                lot.value = reponse.data;
            }
        } catch (error) {
            console.error("Erreur lors du rechargement des données du lot:", error);
        }
    };

    const estAdmin = computed(() => store.getters.isAdmin);
    const estConnecte = computed(() => store.state.isLoggedIn);

    const gererCliquerSurMiser = (evenement) => {
        if (!estConnecte.value) {
            toast.info("Veuillez vous connecter pour pouvoir miser", {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: false,
                pauseOnHover: true,
                draggable: true,
                theme: "colored",
                style: {
                    fontSize: '16px',
                    padding: '15px'
                },
                onClose: () => {
                    router.push('/connexion');
                }
            });
            return;
        }
        ouvrirModalMise(evenement);
    };

    // Watches
    watch(() => store.state.lots, (newLots) => {
        const lotActuel = store.getters.getLot(props.lotRecu.id);
        if (lotActuel) {
            lot.value = lotActuel;
        }
    }, { deep: true });

    watch(() => store.state.lots, () => {
        estUtilisateurPlusHauteMise.value;
    }, { immediate: true, deep: true });

    watch(estConnecte, (newValue) => {
        if (!newValue) {
            const lotActuel = store.getters.getLot(props.lotRecu.id);
            if (lotActuel) {
                lot.value = lotActuel;
            }
        }
    });

    watch(() => store.getters.getLot(props.lotRecu.id)?.mise, async (newMise) => {
        if (newMise && props.lotRecu.id) {
            userLastBid.value = await store.dispatch('getUserBidForLot', props.lotRecu.id);
        }
    });

    // Lifecycle hooks
    onMounted(async () => {
        lot.value = props.lotRecu;
        urlApi.value = await store.state.api.defaults.baseURL.replace("\api", "");

        // Ne récupérer la dernière mise que si l'utilisateur est connecté
        if (store.state.estConnecte && props.lotRecu.id) {
            userLastBid.value = await store.dispatch('getUserBidForLot', props.lotRecu.id);
        }

        if (store.state.connection) {
            store.state.connection.on("ReceiveNewBid", gererNouvelleEnchere);
        }
    });

    onUnmounted(() => {
        if (store.state.connection) {
            store.state.connection.off("ReceiveNewBid", gererNouvelleEnchere);
        }
    });

    const nombreOffres = computed(() => {
        return store.getters.getUniqueOffersCount(props.lotRecu.id);
    });

    // Remplacer useIntervalFn par un computed plus réactif
    const tempsRestant = computed(() => {
        const lot = store.getters.getLot(props.lotRecu.id);
        if (!lot?.dateFinDecompteLot) return 0;

        // Utiliser performance.now() pour une meilleure précision
        const fin = new Date(lot.dateFinDecompteLot).getTime();
        const maintenant = Date.now();
        return Math.max(0, Math.floor((fin - maintenant) / 1000));
    });

    // Utiliser requestAnimationFrame pour une mise à jour plus fluide
    let rafId = null;

    const mettreAJourCompteur = () => {
        if (tempsRestant.value <= 0) {
            cancelAnimationFrame(rafId);
            
            // Plus d'appel au serveur via SignalR
            // Juste une mise à jour locale via le store
            store.commit('SET_LOT_VENDU', props.lotRecu.id);
            return;
        }
        rafId = requestAnimationFrame(mettreAJourCompteur);
    };

    onMounted(() => {
        rafId = requestAnimationFrame(mettreAJourCompteur);
    });

    onUnmounted(() => {
        if (rafId) cancelAnimationFrame(rafId);
    });

    // Computed pour le format du décompte
    const formatDecompte = computed(() => {
        const minutes = Math.floor(tempsRestant.value / 60);
        const secondes = tempsRestant.value % 60;
        return `${minutes.toString().padStart(2, '0')}:${secondes.toString().padStart(2, '0')}`;
    });

    // Écouter les mises à jour de temps via SignalR
    onMounted(() => {
        if (store.state.connection) {
            store.state.connection.on("ReceiveNewBid", (data) => {
                if (data.type === "tempsLotMisAJour" && data.lotId === props.lotRecu.id) {
                    store.commit('UPDATE_LOT_TEMPS', {
                        lotId: data.lotId,
                        nouveauTemps: new Date(data.nouveauTemps),
                        ordreLotsActuel: data.ordreLotsActuel
                    });
                }
            });
        }
    });

    // Utiliser nextTick pour les mises à jour visuelles
    watch(tempsRestant, async (newValue) => {
        if (newValue <= 0) {
            await nextTick();
            // Autres actions...
        }
    });

    // Computed pour vérifier si on peut miser
    const peutMiser = computed(() => {
        const lot = store.getters.getLot(props.lotRecu.id);
        if (!lot) return false;

        // Vérifier si le lot a une date de fin de décompte
        if (!lot.dateFinDecompteLot) return false;

        // Vérifier si le temps est écoulé
        const maintenant = new Date();
        const finDecompte = new Date(lot.dateFinDecompteLot);

        return !lot.estVendu && finDecompte > maintenant;
    });



</script>

<style scoped>
    .fs-7 {
        font-size: 0.89rem;
    }

    .fs-8 {
        font-size: 0.65rem;
    }

    .fs-9 {
        font-size: 0.80rem;
    }

    .card {
        box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.2), 0 3px 5px 0 rgba(0, 0, 0, 0.19);
        transition: border-color 0.3s ease, box-shadow 0.3s ease;
    }

    .user-bid {
        border: 2px solid #4CAF50 !important;
        box-shadow: 0 0 10px rgba(76, 175, 80, 0.3) !important;
    }

    .user-outbid {
        border: 2px solid #FF0000 !important;
        box-shadow: 0 0 10px rgba(255, 0, 0, 0.3) !important;
    }
</style>
