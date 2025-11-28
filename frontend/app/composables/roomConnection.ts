import { ref, onMounted, onUnmounted } from "vue";
import type { HubConnection } from "@microsoft/signalr";
import { HubConnectionBuilder } from "@microsoft/signalr";

export function useRoomConnection(roomId: string) {
  const connection = ref<HubConnection | null>(null);

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
  });

  onUnmounted(async () => {
    if (connection.value) {
      await connection.value.stop();
    }
  });

  return { connection };
}
