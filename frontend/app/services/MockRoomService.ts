import type { IRoomService } from "~~/shared/types/roomService";

export class MockRoomService implements IRoomService {
  async getRoom(id: string) {
    return id === "1234" ? { id } : null;
  }
}
