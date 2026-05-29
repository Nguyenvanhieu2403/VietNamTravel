import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../services/api.service';
import { RegionDto } from '../models/travel.models';

@Injectable({
  providedIn: 'root'
})
export class RegionService {
  constructor(private apiService: ApiService) {}

  getRegions(): Observable<RegionDto[]> {
    return this.apiService.get<RegionDto[]>('regions');
  }

  getRegionBySlug(slug: string): Observable<RegionDto> {
    return this.apiService.get<RegionDto>(`regions/${slug}`);
  }
}
