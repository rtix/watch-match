export interface IRoomService {
  getRoom(id: string): Promise<{ id: string } | null>;
}
