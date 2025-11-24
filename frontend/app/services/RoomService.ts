import type { IRoomService } from "~~/shared/types/roomService";

export class RoomService implements IRoomService {
  constructor() {
    throw Error("Not implemented service.");
  }
  async getRoom(id: string) {
    return { id };
  }
}
