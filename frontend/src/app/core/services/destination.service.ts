import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../services/api.service';
import { DestinationDto, PaginatedList } from '../models/travel.models';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DestinationService {
  constructor(private apiService: ApiService) {}

  getDestinations(pageNumber: number = 1, pageSize: number = 10, provinceId?: number, regionId?: number, category?: string): Observable<PaginatedList<DestinationDto>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (provinceId) {
      params = params.set('provinceId', provinceId.toString());
    }
    if (regionId) {
      params = params.set('regionId', regionId.toString());
    }
    if (category) {
      params = params.set('category', category);
    }

    return this.apiService.get<PaginatedList<DestinationDto>>('destinations', params);
  }

  getFeaturedDestinations(limit: number = 10): Observable<DestinationDto[]> {
    const params = new HttpParams().set('limit', limit.toString());
    return this.apiService.get<DestinationDto[]>('destinations/featured', params);
  }

  getDestinationBySlug(slug: string): Observable<DestinationDto> {
    return this.apiService.get<DestinationDto>(`destinations/${slug}`);
  }

  getDestinationsByRegion(regionId: number, pageNumber: number = 1, pageSize: number = 10): Observable<PaginatedList<DestinationDto>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this.apiService.get<PaginatedList<DestinationDto>>(`destinations/by-region/${regionId}`, params);
  }

  getDestinationsByProvince(provinceId: number, pageNumber: number = 1, pageSize: number = 10): Observable<PaginatedList<DestinationDto>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this.apiService.get<PaginatedList<DestinationDto>>(`destinations/by-province/${provinceId}`, params);
  }
}
