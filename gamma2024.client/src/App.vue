<script setup>
import BarreDeNavigation from "./components/views/BarreDeNavigation.vue";
import PiedDePage from "./components/views/PiedDePage.vue";
import { onMounted, onBeforeMount } from "vue";
import { useStore } from "vuex";

const store = useStore();

onBeforeMount(async () => {
    const token = localStorage.getItem("token");
    if (token && !store.state.isLoggedIn) {
        await store.dispatch("checkAuthStatus");
    }
});

onMounted(async () => {
    if (store.state.isLoggedIn) {
        await store.dispatch("fetchUserBids");
        if (!store.state.connection) {
            await store.dispatch("initializeSignalR");
        }
    }
});
</script>

<template>
  <BarreDeNavigation />

  <main class="mh-100 flex-grow-1">
    <router-view></router-view>
  </main>

  <footer>
    <PiedDePage />
  </footer>

  <Toaster position="top-right" />
</template>

<script>
export default {
  name: "App",
};
</script>

<style scoped></style>
