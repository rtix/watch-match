<template>
  <div>
    <h1>Room ID: {{ roomId }}</h1>
    <button @click="requestMore">Request movies</button>
    <p v-for="m in movies" :key="m.title">
      {{ m.title }}
      <button @click="sendLike(m.id)">❤︎</button>
      <button style="margin-left: 0.25rem" @click="sendDislike(m.id)">
        Dislike
      </button>
    </p>
  </div>
</template>

<script setup lang="ts">
const route = useRoute();
const roomId = Array.isArray(route.params.id)
  ? route.params.id[0] ?? ""
  : route.params.id ?? "";
const movies = ref<{ title: string; id: number }[]>([]);

async function requestMore() {
  movies.value = await requestMovies();
}

const { requestMovies, sendLike, sendDislike } = useRoomConnection(roomId);
</script>
