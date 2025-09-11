<template>
    <footer class="bleuMarinSecondaireFond text-white py-1 justify-content-between d-flex align-items-center flex-column flex-md-row">

        <p class="col-md-4 col-lg-5 mb-0 ps-3">
            © Les Encans de Nantes au Québec {{ new Date().getFullYear() }} par René Tchokomi
        </p>

        <div class="col-md-8 col-lg-7">
            <ul class="nav justify-content-end d-flex align-items-center flex-column flex-md-row">
                <li v-if="!estAdmin">
                    <router-link :to="{ name: 'ManuelUtilisateur' }" class="text-decoration-none">
                        <a class="nav-link text-white">
                            Manuel de l'utilisateur
                        </a>
                    </router-link>
                </li>
                <li>
                    <router-link :to="{ name: 'ConditionsCompagnie' }" class="text-decoration-none">
                        <a class="nav-link text-white">
                            Conditions de la compagnie
                        </a>
                    </router-link>
                </li>
                <li>
                    <router-link :to="{name: 'Contact'}" class="text-decoration-none">
                        <a class="nav-link text-white">
                            Contact
                        </a>
                    </router-link>
                </li>
                <li>
                    <router-link :to="{name: 'APropos'}" class="text-decoration-none">
                        <a class="nav-link text-white">
                            À propos
                        </a>
                    </router-link>
                </li>
            </ul>
        </div>

    </footer>
</template>

<script setup>
    import { computed } from "vue";
    import { useStore } from "vuex";

    const store = useStore();
    const estAdmin = computed(() => store.getters.isAdmin);
</script>

<style scoped>
</style>