export interface IRoomService {
  createRoom(userId: string): Promise<string | null>;
  signToRoom(roomId: string, userId: string): Promise<boolean>;
}
