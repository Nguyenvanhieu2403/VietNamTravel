import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { CultureDto, PaginatedList } from '../models/travel.models';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CultureService {
  constructor(private apiService: ApiService) {}

  getCultures(pageNumber: number = 1, pageSize: number = 10, regionId?: number, cultureType?: string): Observable<PaginatedList<CultureDto>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (regionId) {
      params = params.set('regionId', regionId.toString());
    }
    if (cultureType) {
      params = params.set('cultureType', cultureType);
    }

    return this.apiService.get<PaginatedList<CultureDto>>('cultures', params);
  }

  getFeaturedCultures(limit: number = 10): Observable<CultureDto[]> {
    const params = new HttpParams().set('limit', limit.toString());
    return this.apiService.get<CultureDto[]>('cultures/featured', params);
  }

  getCultureBySlug(slug: string): Observable<CultureDto> {
    return this.apiService.get<CultureDto>(`cultures/${slug}`);
  }

  getCulturesByRegion(regionId: number, pageNumber: number = 1, pageSize: number = 10): Observable<PaginatedList<CultureDto>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this.apiService.get<PaginatedList<CultureDto>>(`cultures/by-region/${regionId}`, params);
  }
}
