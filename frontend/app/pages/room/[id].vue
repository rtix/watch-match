<template>
  <div>
    <h1>Room ID: {{ roomId }}</h1>
    <Tabs.Root v-model="tabState">
      <Tabs.List>
        <Tabs.Trigger value="discover">Discover</Tabs.Trigger>
        <Tabs.Trigger value="results">Results</Tabs.Trigger>
      </Tabs.List>

      <Tabs.Content value="discover">
        <button @click="requestMore">Request movies</button>
        <p v-for="m in movies" :key="m.title">
          <img
            :src="`${useRuntimeConfig().public.tmdbImageBase}/original${
              m.posterPath
            }`"
            alt="Movie Poster"
            style="height: 5rem"
          />
          {{ m.title }}
          <button @click="sendLike(m.id)">❤︎</button>
          <button style="margin-left: 0.25rem" @click="sendDislike(m.id)">
            Dislike
          </button>
        </p>
      </Tabs.Content>

      <Tabs.Content value="results">
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
      </Tabs.Content>
    </Tabs.Root>
  </div>
</template>

<script setup lang="ts">
import { Tabs } from "@ark-ui/vue/tabs";
import type { IMovie } from "~~/shared/types/roomService";

const route = useRoute();

const roomId = Array.isArray(route.params.id)
  ? route.params.id[0] ?? ""
  : route.params.id ?? "";
const { likes, requestMovies, sendLike, sendDislike } =
  useRoomConnection(roomId);

const movies = ref<IMovie[]>([]);
const tabState = ref("discover");

async function requestMore() {
  movies.value = await requestMovies();
}
</script>
