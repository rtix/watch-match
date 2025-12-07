import { ref, onMounted, onUnmounted } from "vue";
import type { HubConnection } from "@microsoft/signalr";
import { HubConnectionBuilder } from "@microsoft/signalr";
import type { IMovieLikesDTO } from "~~/shared/types/roomService";

export function useRoomConnection(roomId: string) {
  const connection = ref<HubConnection | null>(null);
  const likes = ref<IMovieLikesDTO[]>([]);

  let readyResolve: () => void;
  const ready = new Promise<void>((resolve) => {
    readyResolve = resolve;
  });

  onMounted(async () => {
    connection.value = new HubConnectionBuilder()
      .withUrl(
        `${useRuntimeConfig().public.apiBase}/room-hub?roomId=${roomId}`,
        {
          accessTokenFactory: () => useNuxtApp().$guestId,
        }
      )
      .withAutomaticReconnect()
      .build();

    await connection.value.start();
    readyResolve();

    connection.value.on("RecieveLikes", (l: IMovieLikesDTO[]) => {
      likes.value = l;
    });
  });

  const requestInProgress = ref(false);

  async function requestMovies() {
    await ready;
    if (requestInProgress.value) return [];

    requestInProgress.value = true;
    try {
      const movies = await connection.value?.invoke("RequestMovies");
      return movies || [];
    } finally {
      requestInProgress.value = false;
    }
  }

  function sendLike(movieId: number) {
    connection.value?.invoke("SendLike", movieId);
  }

  function sendDislike(movieId: number) {
    connection.value?.invoke("SendDislike", movieId);
  }

  onUnmounted(async () => {
    if (connection.value) {
      await connection.value.stop();
    }
  });

  return { connection, requestMovies, sendLike, sendDislike, likes };
}
