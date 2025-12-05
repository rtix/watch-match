export interface IRoomService {
  createRoom(userId: string): Promise<string | null>;
  signToRoom(roomId: string, userId: string): Promise<boolean>;
}

export interface IMovie {
  id: int;
  title: string;
}

export interface IMovieLikesDTO {
  movie: IMovie;
  likes: number;
}
