<template>
  <div class="room">
    <div v-show="!isScoreState" class="room__discover room__page">
      <ui-spinner v-if="currentMovie === null" class="room__spinner" />
      <movie-flip-card
        v-else
        :movie="currentMovie"
        class="room__movie-viewer"
      />
    </div>

    <div v-show="isScoreState" class="room__score room__page">
      <div v-for="m in likes" :key="m.movie.id">
        <img
          :src="`${useRuntimeConfig().public.tmdbImageBase}/original${
            m.movie.posterPath
          }`"
          alt="Movie Poster"
          style="height: 5rem; display: inline"
        />
        {{ m.movie.title }} {{ m.likes }}
      </div>
    </div>

    <div class="room__controls">
      <button @click="isScoreState = !isScoreState">Score</button>
      <button v-show="!isScoreState" @click="react('like')">Like</button>
      <button v-show="!isScoreState" @click="react('dislike')">Dislike</button>
      <button v-show="!isScoreState" @click="copyRoomId()">Room ID</button>
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { MovieDto } from "~~/shared/types/generated/MovieDto";

definePageMeta({
  layoutMeta: { limitHeightToViewport: true },
});

const isScoreState = ref(false);
const route = useRoute();

const roomId = Array.isArray(route.params.id)
  ? route.params.id[0] ?? ""
  : route.params.id ?? "";

const { likes, requestMovies, sendLike, sendDislike } =
  useRoomConnection(roomId);

const movies = ref<MovieDto[]>([]);
const currentMovie = computed(() => movies.value[0] || null);

async function requestMore() {
  const more = await requestMovies();
  movies.value.push(...more);
}

function react(reactionType: "like" | "dislike") {
  if (currentMovie.value === null) {
    return;
  }
  const send = {
    like: sendLike,
    dislike: sendDislike,
  };
  send[reactionType](currentMovie.value.id);
  movies.value.shift();
}

async function copyRoomId() {
  await navigator.clipboard.writeText(roomId);
}

watch(
  () => movies.value.length,
  (len) => {
    const moviesToPreload = [movies.value[1], movies.value[2]];
    moviesToPreload.forEach((m) => {
      if (m) {
        preloadImage(
          `${useRuntimeConfig().public.tmdbImageBase}/original${m.posterPath}`
        );
      }
    });

    if (len < 6) requestMore();
  },
  { immediate: true }
);
</script>

<style scoped>
.room {
  flex: 1;
  position: relative;
}

.room__controls {
  position: absolute;
  right: 0;
  display: flex;
  flex-direction: column;
}

.room__page {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  overflow: scroll;
}

.room__discover {
}

.room__poster {
  cursor: pointer;
  object-fit: contain;
  width: 100%;
  height: 100%;
}

.room__more-info {
  min-height: 100%;
  cursor: pointer;
}

.room__spinner {
  width: 100%;
  height: 100%;
}

.room__movie-viewer {
  width: 100%;
  height: 100%;
}
</style>
