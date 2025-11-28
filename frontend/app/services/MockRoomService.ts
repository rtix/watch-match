import type { IRoomService } from "~~/shared/types/roomService";

export class MockRoomService implements IRoomService {
  async createRoom(userId: string) {
    return { id: crypto.randomUUID(), creatorId: userId };
  }
}
