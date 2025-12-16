<template>
  <div>
    <NuxtImg
      :src="
        movie.posterPath &&
        `${useRuntimeConfig().public.tmdbImageBase}/original${movie.posterPath}`
      "
      placeholder="/images/placeholder.jpg"
      style="height: 5rem; display: inline"
    />

    {{ movie.title }} {{ likes }}
  </div>
</template>

<script lang="ts" setup>
import type { MovieDto } from "~~/shared/types/generated/MovieDto";
defineProps<{ movie: MovieDto; likes?: number }>();
</script>

<style scoped></style>
