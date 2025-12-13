<template>
  <div class="movie-viewer">
    <NuxtImg
      v-show="!isMoreInfoState"
      :key="movie?.posterPath"
      class="movie-viewer__poster"
      :src="`${useRuntimeConfig().public.tmdbImageBase}/original${
        movie?.posterPath
      }`"
      placeholder="/images/placeholder.jpg"
      @click="isMoreInfoState = !isMoreInfoState"
    />
    <div
      v-show="isMoreInfoState"
      class="movie-viewer__more-info"
      @click="isMoreInfoState = !isMoreInfoState"
    >
      <h1>{{ movie?.title }}</h1>
      {{ movie }}
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { MovieDto } from "~~/shared/types/generated/MovieDto";
defineProps<{ movie: MovieDto }>();
const isMoreInfoState = ref(false);
</script>

<style scoped>
.movie-viewer__poster {
  cursor: pointer;
  object-fit: contain;
  width: 100%;
  height: 100%;
}

.movie-viewer__more-info {
  min-height: 100%;
  cursor: pointer;
}
</style>
