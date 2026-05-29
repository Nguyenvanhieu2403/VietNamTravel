import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ProvinceService } from '../../core/services/province.service';
import { ProvinceDto } from '../../core/models/travel.models';

@Component({
  selector: 'app-province-detail',
  templateUrl: './province-detail.html',
  styleUrl: './province-detail.scss',
  standalone: false,
})
export class ProvinceDetailComponent implements OnInit {
  private isBrowser: boolean;
  public provinceSlug: string = '';
  public province: ProvinceDto | null = null;
  public loading: boolean = true;
  public error: string | null = null;

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private route: ActivatedRoute,
    private provinceService: ProvinceService,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.provinceSlug = params['slug'];
      this.loadProvinceDetail();
    });
  }

  private loadProvinceDetail(): void {
    this.loading = true;
    this.provinceService.getProvinceBySlug(this.provinceSlug).subscribe({
      next: (data) => {
        this.province = data;
        this.loading = false;
        if (this.isBrowser) {
          setTimeout(() => this.initAnimations(), 100);
        }
      },
      error: (err) => {
        this.error = 'Không thể tải thông tin tỉnh thành. Vui lòng thử lại sau.';
        this.loading = false;
        console.error('Error loading province detail:', err);
      },
    });
  }

  private async initAnimations(): Promise<void> {
    try {
      const { gsap } = await import('gsap');
      const { ScrollTrigger } = await import('gsap/ScrollTrigger');

      gsap.registerPlugin(ScrollTrigger);

      gsap.from('.hero-content', {
        y: 80,
        opacity: 0,
        duration: 1.2,
        ease: 'power3.out',
      });

      gsap.utils.toArray<HTMLElement>('.fade-in-section').forEach((section) => {
        gsap.from(section, {
          scrollTrigger: {
            trigger: section,
            start: 'top 80%',
            toggleActions: 'play none none none',
          },
          y: 60,
          opacity: 0,
          duration: 0.8,
          ease: 'power2.out',
        });
      });
    } catch (err) {
      console.warn('Could not load GSAP: ', err);
    }
  }
}
