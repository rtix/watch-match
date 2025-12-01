import type { IRoomService } from "~~/shared/types/roomService";
import { FetchError } from "ofetch";

export class RoomService implements IRoomService {
  async createRoom(userId: string) {
    try {
      const roomId = await $fetch<string | null>(
        `${useRuntimeConfig().public.apiBase}/api/room`,
        {
          method: "POST",
          body: JSON.stringify(userId),
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      return roomId;
    } catch {
      return null;
    }
  }

  async signToRoom(roomId: string, userId: string) {
    try {
      await $fetch<IRoom>(
        `${useRuntimeConfig().public.apiBase}/api/room/${roomId}/users`,
        {
          method: "PATCH",
          body: JSON.stringify(userId),
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      return true;
    } catch (err: unknown) {
      if (err instanceof FetchError) {
        switch (err.status) {
          case 403:
            console.log("Forbidden");
            break;
          case 404:
            console.log("Not found");
            break;
        }
      }
      return false;
    }
  }
}
