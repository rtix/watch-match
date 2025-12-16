<template>
  <div class="movie-score-card">
    <NuxtImg
      class="movie-score-card__poster"
      :src="
        movie.posterPath &&
        `${useRuntimeConfig().public.tmdbImageBase}/original${movie.posterPath}`
      "
      placeholder="/images/placeholder.jpg"
    />
    <div class="movie-score-card__info">
      <div class="movie-score-card__title">
        {{ movie.title }}
        <span v-if="likes" class="movie-score-card__likes">
          {{ likes }} <Icon name="material-symbols:favorite" />
        </span>
      </div>
      <p>{{ movie.overview }}</p>
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { MovieDto } from "~~/shared/types/generated/MovieDto";
defineProps<{ movie: MovieDto; likes?: number }>();
</script>

<style scoped>
.movie-score-card {
  display: flex;
  align-items: center;

  padding-block: var(--spacing-xs);
}

.movie-score-card__poster {
  width: var(--spacing-3xl);
}

.movie-score-card__info {
  padding: var(--spacing-sm);
}

.movie-score-card__title {
  font-weight: bold;
}

.movie-score-card__likes {
  display: inline-flex;
  align-items: center;
  color: var(--color-secondary);
}
</style>
