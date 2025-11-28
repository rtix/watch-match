import type { IRoom, IRoomService } from "~~/shared/types/roomService";

export class RoomService implements IRoomService {
  async createRoom(userId: string) {
    return await $fetch<IRoom>(
      `${useRuntimeConfig().public.apiBase}/api/room`,
      {
        method: "POST",
        body: JSON.stringify(userId),
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
  }
}
