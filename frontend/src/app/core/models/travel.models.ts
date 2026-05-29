export interface RegionDto {
  id: number;
  name: string;
  slug: string;
  description?: string;
  imageUrl?: string;
  highlights?: string[];
  bestSeason?: string;
  provinces: ProvinceListDto[];
}

export interface ProvinceListDto {
  id: number;
  name: string;
  slug: string;
  description?: string;
  bestTimeToVisit?: string;
  averageBudget: number;
  thumbnailUrl?: string;
  imageUrl?: string;
}

export interface ProvinceDto {
  id: number;
  regionId: number;
  regionName: string;
  name: string;
  slug: string;
  description?: string;
  cultureDescription?: string;
  bestTimeToVisit?: string;
  averageBudget: number;
  videoUrl?: string;
  thumbnailUrl?: string;
  destinations: DestinationDto[];
  foods: FoodDto[];
  festivals: FestivalDto[];
  seasons: TravelSeasonDto[];
  reviews: ReviewDto[];
  mediaFiles: MediaFileDto[];
}

export interface DestinationDto {
  id: number;
  name: string;
  slug: string;
  description?: string;
  shortDescription?: string;
  thumbnailUrl?: string;
  bannerUrl?: string;
  provinceId: number;
  provinceName?: string;
  regionId: number;
  regionName?: string;
  category?: string;
  bestTimeToVisit?: string;
  estimatedBudget?: number;
  latitude?: number;
  longitude?: number;
  rating?: number;
  isFeatured: boolean;
  mediaFiles: MediaFileDto[];
}

export interface FoodDto {
  id: number;
  name: string;
  description?: string;
  recipeLink?: string;
  thumbnailUrl?: string;
}

export interface FestivalDto {
  id: number;
  name: string;
  description?: string;
  heldDate?: string;
  lunarDate?: string;
}

export interface TravelSeasonDto {
  id: number;
  seasonName: string;
  months?: string;
  weatherCondition?: string;
  tips?: string;
}

export interface ReviewDto {
  id: number;
  username: string;
  userFullName: string;
  destinationId?: number;
  provinceId?: number;
  rating: number;
  comment?: string;
  createdAt: Date;
}

export interface CreateReviewRequest {
  destinationId?: number;
  provinceId?: number;
  rating: number;
  comment?: string;
}

export interface BlogDto {
  id: number;
  title: string;
  slug: string;
  summary?: string;
  content: string;
  thumbnailUrl?: string;
  bannerUrl?: string;
  author?: string;
  tags?: string;
  readTime?: number;
  viewCount: number;
  isFeatured: boolean;
  publishedAt?: Date;
  createdAt: Date;
  mediaFiles: MediaFileDto[];
}

export interface CultureDto {
  id: number;
  title: string;
  slug: string;
  description?: string;
  content?: string;
  thumbnailUrl?: string;
  bannerUrl?: string;
  regionId?: number;
  regionName?: string;
  cultureType?: string;
  festivalSeason?: string;
  isFeatured: boolean;
  createdAt: Date;
  mediaFiles: MediaFileDto[];
}

export interface CreateBlogRequest {
  title: string;
  content: string;
  isPublished: boolean;
}

export interface MediaFileDto {
  id: number;
  url: string;
  fileType: string;
}

export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
