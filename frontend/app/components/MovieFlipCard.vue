<template>
  <div class="movie-flip-card">
    <NuxtImg
      v-show="!isMoreInfoState"
      :key="movie?.posterPath"
      class="movie-flip-card__poster"
      :src="posterPath"
      placeholder="/images/placeholder.jpg"
      @click="isMoreInfoState = !isMoreInfoState"
    />
    <div
      v-show="isMoreInfoState"
      class="movie-flip-card__more-info"
      @click="isMoreInfoState = !isMoreInfoState"
    >
      <h1>
        {{ movie.title }}
        <span>
          {{ movie.voteAverage.toFixed(1) }}
          <sup style="font-weight: normal">{{ movie.voteCount }}</sup>
        </span>
      </h1>
      <p class="movie-flip-card__genres">
        <span
          v-for="genre in movie.genres"
          :key="genre.id"
          class="movie-flip-card__genre"
        >
          {{ genre.name }}
        </span>
      </p>
      <p>{{ movie.overview }}</p>
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { MovieDto } from "~~/shared/types/generated/MovieDto";
const { movie } = defineProps<{ movie: MovieDto }>();
const isMoreInfoState = ref(false);
const posterPath = computed(
  () =>
    movie.posterPath &&
    `${useRuntimeConfig().public.tmdbImageBase}/original${movie.posterPath}`
);
</script>

<style scoped>
.movie-flip-card {
  display: flex;
}

.movie-flip-card__poster {
  cursor: pointer;
  object-fit: contain;
  width: 100%;
  height: 100%;
}

.movie-flip-card__more-info {
  min-height: 100%;
  cursor: pointer;
}

.movie-flip-card__genre {
  /* padding-inline: var(--spacing-xs); */
}

.movie-flip-card__genres {
  display: flex;
  gap: var(--spacing-xs);
}
</style>
