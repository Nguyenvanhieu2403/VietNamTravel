import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../services/api.service';
import { ProvinceListDto, ProvinceDto, PaginatedList } from '../models/travel.models';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProvinceService {
  constructor(private apiService: ApiService) {}

  getProvinces(pageNumber: number = 1, pageSize: number = 10, regionId?: number): Observable<PaginatedList<ProvinceListDto>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (regionId) {
      params = params.set('regionId', regionId.toString());
    }

    return this.apiService.get<PaginatedList<ProvinceListDto>>('provinces', params);
  }

  getProvinceBySlug(slug: string): Observable<ProvinceDto> {
    return this.apiService.get<ProvinceDto>(`provinces/${slug}`);
  }
}
