import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../services/api.service';
import { BlogDto, CreateBlogRequest, PaginatedList } from '../models/travel.models';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  constructor(private apiService: ApiService) {}

  getBlogs(pageNumber: number = 1, pageSize: number = 10, category?: string): Observable<PaginatedList<BlogDto>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (category) {
      params = params.set('category', category);
    }

    return this.apiService.get<PaginatedList<BlogDto>>('blogs', params);
  }

  getFeaturedBlogs(limit: number = 10): Observable<BlogDto[]> {
    const params = new HttpParams().set('limit', limit.toString());
    return this.apiService.get<BlogDto[]>('blogs/featured', params);
  }

  getLatestBlogs(limit: number = 10): Observable<BlogDto[]> {
    const params = new HttpParams().set('limit', limit.toString());
    return this.apiService.get<BlogDto[]>('blogs/latest', params);
  }

  getBlogBySlug(slug: string): Observable<BlogDto> {
    return this.apiService.get<BlogDto>(`blogs/${slug}`);
  }

  createBlog(blog: CreateBlogRequest): Observable<number> {
    return this.apiService.post<number>('blogs', blog);
  }
}
