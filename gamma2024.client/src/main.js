import "./assets/main.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import PrimeVue from "primevue/config";
import Toast from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import InputMask from "primevue/inputmask";
import VueDatePicker from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";

async function initApp() {
  //await store.dispatch("initializeStore");

  const app = createApp(App);
  app.component("VueDatePicker", VueDatePicker);
  app.use(store).use(router).use(PrimeVue, { theme: "none" }).use(Toast);
  app.component("InputMask", InputMask);
  await router.isReady();
  app.mount("#app");
}

initApp().catch((error) =>
  console.error("Erreur lors de l'initialisation de l'app:", error)
);
