<template>
  <div class="room">
    <div v-show="!isScoreState" class="room__discover room-page">
      <img
        v-show="!isMoreInfoState"
        class="room__poster"
        :src="`${useRuntimeConfig().public.tmdbImageBase}/original${
          currentMovie?.posterPath
        }`"
        @click="isMoreInfoState = !isMoreInfoState"
      />
      <div
        v-show="isMoreInfoState"
        class="room__more-info"
        @click="isMoreInfoState = !isMoreInfoState"
      >
        {{ currentMovie }}
      </div>
    </div>

    <div v-show="isScoreState" class="room__score room-page">
      <div v-for="m in likes" :key="m.movie.id">
        <img
          :src="`${useRuntimeConfig().public.tmdbImageBase}/original${
            m.movie.posterPath
          }`"
          alt="Movie Poster"
          style="height: 5rem"
        />
        {{ m.movie.title }} {{ m.likes }}
      </div>
    </div>

    <div class="room__controls">
      <button @click="isScoreState = !isScoreState">Score</button>
      <button @click="like()">Like</button>
      <button @click="dislike()">Dislike</button>
    </div>
  </div>
</template>

<script lang="ts" setup>
definePageMeta({
  layoutMeta: { limitHeightToViewport: true },
});

const isMoreInfoState = ref(false);
const isScoreState = ref(false);

const route = useRoute();

const roomId = Array.isArray(route.params.id)
  ? route.params.id[0] ?? ""
  : route.params.id ?? "";

const { likes, requestMovies, sendLike, sendDislike } =
  useRoomConnection(roomId);

const movies = ref<IMovie[]>([]);
const currentMovie = computed(() => movies.value[0] || null);

async function requestMore() {
  const more = await requestMovies();
  console.log(more);
  movies.value.push(...more);
}

function like() {
  if (currentMovie.value === null) {
    return;
  }
  sendLike(currentMovie.value.id);
  movies.value.shift();
}

function dislike() {
  if (currentMovie.value === null) {
    return;
  }
  sendDislike(currentMovie.value.id);
  movies.value.shift();
}

function preloadImage(url: string) {
  const img = new Image();
  img.src = url;
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

.room-page {
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
</style>
