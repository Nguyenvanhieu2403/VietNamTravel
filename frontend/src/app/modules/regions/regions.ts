import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { RegionService } from '../../core/services/region.service';
import { RegionDto } from '../../core/models/travel.models';

@Component({
  selector: 'app-regions',
  templateUrl: './regions.html',
  styleUrl: './regions.scss',
  standalone: false
})
export class RegionsComponent implements OnInit {
  private isBrowser: boolean;
  public regions: RegionDto[] = [];
  public loading: boolean = true;
  public error: string | null = null;

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private regionService: RegionService
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.loadRegions();
  }

  private loadRegions(): void {
    this.loading = true;
    this.regionService.getRegions().subscribe({
      next: (data) => {
        this.regions = data;
        this.loading = false;
        if (this.isBrowser) {
          setTimeout(() => this.initAnimations(), 100);
        }
      },
      error: (err) => {
        this.error = 'Không thể tải dữ liệu vùng miền. Vui lòng thử lại sau.';
        this.loading = false;
        console.error('Error loading regions:', err);
      }
    });
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

      gsap.utils.toArray<HTMLElement>('.region-card').forEach((card, index) => {
        gsap.from(card, {
          scrollTrigger: {
            trigger: card,
            start: 'top 85%',
            toggleActions: 'play none none none'
          },
          y: 80,
          opacity: 0,
          duration: 0.8,
          delay: index * 0.1,
          ease: 'power2.out'
        });
      });
    } catch (err) {
      console.warn('Could not load GSAP: ', err);
    }
  }
}
