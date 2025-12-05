import { ref, onMounted, onUnmounted } from "vue";
import type { HubConnection } from "@microsoft/signalr";
import { HubConnectionBuilder } from "@microsoft/signalr";
import type { IMovieLikesDTO } from "~~/shared/types/roomService";

export function useRoomConnection(roomId: string) {
  const connection = ref<HubConnection | null>(null);
  const likes = ref<IMovieLikesDTO[]>([]);

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
    connection.value.start();

    connection.value.on("ReceiveMessage", (msg) => {
      console.log(msg);
    });

    connection.value.on("ReceiveError", (err) => {
      console.log(err);
    });

    connection.value.on("RecieveLikes", (l: IMovieLikesDTO[]) => {
      likes.value = l;
    });
  });

  async function requestMovies() {
    return await connection.value?.invoke("RequestMovies");
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
