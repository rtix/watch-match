export interface IRoom {
  id: string;
  creatorId: string;
}

export interface IRoomService {
  createRoom(userId: string): Promise<IRoom>;
}
