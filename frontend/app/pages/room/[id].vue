<template>
  <h1>Room ID: {{ roomId }}</h1>
</template>

<script setup>
const route = useRoute();
const roomId = route.params.id;
</script>
