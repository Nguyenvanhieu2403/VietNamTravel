import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ProvinceService } from '../../core/services/province.service';
import { ProvinceListDto } from '../../core/models/travel.models';

@Component({
  selector: 'app-provinces',
  templateUrl: './provinces.html',
  styleUrl: './provinces.scss',
  standalone: false
})
export class ProvincesComponent implements OnInit {
  private isBrowser: boolean;
  public selectedRegion: string = 'all';
  public searchQuery: string = '';
  public provinces: ProvinceListDto[] = [];
  public loading: boolean = true;
  public error: string | null = null;
  public currentPage: number = 1;
  public pageSize: number = 20;
  public totalPages: number = 1;

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private route: ActivatedRoute,
    private provinceService: ProvinceService
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['region']) {
        this.selectedRegion = params['region'];
      }
    });

    this.loadProvinces();
  }

  private loadProvinces(): void {
    this.loading = true;
    const regionId = this.selectedRegion !== 'all' ? parseInt(this.selectedRegion) : undefined;

    this.provinceService.getProvinces(this.currentPage, this.pageSize, regionId).subscribe({
      next: (data) => {
        this.provinces = data.items;
        this.totalPages = data.totalPages;
        this.loading = false;
        if (this.isBrowser) {
          setTimeout(() => this.initAnimations(), 100);
        }
      },
      error: (err) => {
        this.error = 'Không thể tải dữ liệu tỉnh thành. Vui lòng thử lại sau.';
        this.loading = false;
        console.error('Error loading provinces:', err);
      }
    });
  }

  get filteredProvinces(): ProvinceListDto[] {
    if (!this.searchQuery) {
      return this.provinces;
    }
    return this.provinces.filter(province =>
      province.name.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  filterByRegion(region: string): void {
    this.selectedRegion = region;
    this.currentPage = 1;
    this.loadProvinces();
  }

  private async initAnimations(): Promise<void> {
    try {
      const { gsap } = await import('gsap');
      const { ScrollTrigger } = await import('gsap/ScrollTrigger');

      gsap.registerPlugin(ScrollTrigger);

      gsap.from('.hero-title', {
        y: 60,
        opacity: 0,
        duration: 1,
        ease: 'power3.out'
      });

      gsap.utils.toArray<HTMLElement>('.province-card').forEach((card, index) => {
        gsap.from(card, {
          scrollTrigger: {
            trigger: card,
            start: 'top 85%',
            toggleActions: 'play none none none'
          },
          y: 60,
          opacity: 0,
          duration: 0.6,
          delay: index * 0.05,
          ease: 'power2.out'
        });
      });
    } catch (err) {
      console.warn('Could not load GSAP: ', err);
    }
  }
}
