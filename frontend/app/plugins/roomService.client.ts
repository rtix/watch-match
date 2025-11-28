import { MockRoomService } from "~/services/MockRoomService";
import { RoomService } from "~/services/RoomService";
import type { IRoomService } from "~~/shared/types/roomService";

export default defineNuxtPlugin(() => {
  const config = useRuntimeConfig();
  const roomService: IRoomService = config.public.mockBackend
    ? new MockRoomService()
    : new RoomService();

  return { provide: { roomService } };
});
