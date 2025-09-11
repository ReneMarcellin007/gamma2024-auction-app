<template>
    <div class="d-flex w-100 justify-content-center flex-column align-items-center">
        <h1>Enregistrer votre carte</h1>

        <div class="row w-100">
            <form id="payment-form">
                <div class="d-flex align-items-center justify-content-center gap-3" v-if="chargement">
                    <div class="spinner-border" role="status">
                    </div>Chargement...
                </div>
                <div id="payment-element">
                    <!--Stripe.js injects the Payment Element-->
                </div>
                <button id="submit" class="btn btn-outline text-white btnSurvolerBleuMoyenFond my-2">
                    <div class="spinner-grow d-none" id="spinner"></div>
                    <span id="button-text">Enregistrer</span>
                </button>
                <div id="payment-message" class="hidden"></div>
            </form>
        </div>
    </div>
</template>

<script setup>
    import { onMounted, ref } from "vue"
    import { useStore } from "vuex"
    import { useRouter } from "vue-router";

    const store = useStore();

    const router = useRouter()
    const props = defineProps({
        idFacture: String,
    });
    let elements;
    let stripe;
    const chargement = ref(true);

    onMounted(async () => {
        stripe = Stripe(import.meta.env.VITE_STRIPE_PK);
        initialize()

        document.querySelector("#payment-form").addEventListener("submit", soumettre);
    })

    async function initialize() {
        const reponse = await store.dispatch("creerSetupIntent")
        const clientSecret = reponse.data.clientSecret;

        const appearance = {
            theme: 'stripe',

        };
        elements = stripe.elements({ appearance, clientSecret, locale: "fr", loader: 'always' });

        const optionsPaiementElements = {
            layout: "tabs",
        };

        const paymentElement = elements.create("payment", optionsPaiementElements);
        paymentElement.mount("#payment-element");
        chargement.value = false
    }

    async function soumettre(e) {
        e.preventDefault();
        mettreChargement(true);

        const { erreur } = await stripe.confirmSetup({
            elements,
            confirmParams: {
                return_url: "https://google.com",
            },
            redirect: "if_required",
        });

        if (erreur != undefined) {
            if (erreur.type === "card_error" || erreur.type === "validation_error") {
                montrerMessage(erreur.message);
            } else {
                montrerMessage("An unexpected error occurred.");
            }
        }
        else {
            const reponse = await store.dispatch("chercherCartesUser")
            if (reponse.data.length > 1) {
                router.push({ name: 'GestionCartes' })
            }
            else {
                router.push({ name: "EncanPresent" })
            }
        }
        mettreChargement(false);
    }

    function montrerMessage(messageText) {
        const conteneurMessage = document.querySelector("#payment-message");

        conteneurMessage.classList.remove("hidden");
        conteneurMessage.textContent = messageText;

        setTimeout(function () {
            conteneurMessage.classList.add("hidden");
            conteneurMessage.textContent = "";
        }, 4000);
    }

    function mettreChargement(enChargement) {
        if (enChargement) {
            document.querySelector("#submit").classList.add("disabled")
            document.querySelector("#spinner").classList.remove("d-none");
            document.querySelector("#button-text").classList.add("d-none");
        } else {
            document.querySelector("#submit").classList.remove("disabled");
            document.querySelector("#spinner").classList.add("d-none");
            document.querySelector("#button-text").classList.remove("d-none");
        }
    }
</script>

<style scoped>
</style>