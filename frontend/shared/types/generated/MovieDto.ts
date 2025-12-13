/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { Genre } from "./Genre";
import type { Images } from "./Images";
import type { KeywordsContainer } from "./KeywordsContainer";
import type { ResultContainer } from "./ResultContainer";
import type { Video } from "./Video";

export interface MovieDto {
  adult: boolean;
  backdropPath: string;
  budget: number;
  genres: Genre[];
  id: number;
  images: Images;
  imdbId: string;
  keywords: KeywordsContainer;
  originalLanguage: string;
  originalTitle: string;
  overview: string;
  popularity?: number;
  posterPath: string;
  releaseDate?: Date;
  revenue: number;
  runtime?: number;
  tagline: string;
  title: string;
  videos: ResultContainer<Video>;
  voteAverage: number;
  voteCount: number;
}
