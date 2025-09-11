<template>
  <div class="container-fluid bg-danger text-white min-vh-100 d-flex align-items-center">
    <div class="container text-center">
      <h1 class="display-1 fw-bold mb-4">403</h1>
      <h2 class="display-4 mb-3">Accès non autorisé</h2>
      <p class="lead mb-5">Désolé, vous n'avez pas les permissions nécessaires pour accéder à cette page.</p>
      <router-link to="/" class="btn btn-light btn-lg">
        Retour à l'accueil
      </router-link>
    </div>
  </div>
</template>

<script setup>
// Pas de logique nécessaire pour cette page statique
</script>

<style scoped>
.bg-danger {
  background: linear-gradient(45deg, #d9534f, #c9302c);
}
</style>
