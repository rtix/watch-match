import type { IRoomService } from "~~/shared/types/roomService";

export class MockRoomService implements IRoomService {
  async createRoom() {
    return crypto.randomUUID();
  }
  async signToRoom() {
    return true;
  }
}
