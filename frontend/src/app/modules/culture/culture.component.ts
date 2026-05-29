import { Component, OnInit, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { CultureService } from '../../core/services/culture.service';
import { RegionService } from '../../core/services/region.service';
import { CultureDto, RegionDto, FestivalDto, FoodDto } from '../../core/models/travel.models';

@Component({
  selector: 'app-culture',
  templateUrl: './culture.component.html',
  styleUrls: ['./culture.component.scss'],
  standalone: false
})
export class CultureComponent implements OnInit {
  private isBrowser: boolean;

  public loading: boolean = true;
  public error: string | null = null;

  public culturalRegions: RegionDto[] = [];
  public festivals: FestivalDto[] = [];
  public cuisines: FoodDto[] = [];
  public cultures: CultureDto[] = [];

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private cultureService: CultureService,
    private regionService: RegionService
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.loadCultureData();
  }

  private loadCultureData(): void {
    this.loading = true;
    this.error = null;

    // Load regions for cultural regions section
    this.regionService.getRegions().subscribe({
      next: (regions) => {
        this.culturalRegions = regions;
      },
      error: (err) => {
        console.error('Error loading regions:', err);
      }
    });

    // Load featured cultures
    this.cultureService.getFeaturedCultures(10).subscribe({
      next: (cultures) => {
        this.cultures = cultures;

        // Extract festivals and cuisines from cultures
        cultures.forEach(culture => {
          if (culture.cultureType === 'festival') {
            this.festivals.push({
              id: culture.id,
              name: culture.title,
              description: culture.description || '',
              heldDate: culture.festivalSeason || '',
              lunarDate: undefined
            });
          } else if (culture.cultureType === 'cuisine') {
            this.cuisines.push({
              id: culture.id,
              name: culture.title,
              description: culture.description || '',
              thumbnailUrl: culture.thumbnailUrl,
              recipeLink: undefined
            });
          }
        });

        this.loading = false;
        if (this.isBrowser) {
          setTimeout(() => this.initAnimations(), 100);
        }
      },
      error: (err) => {
        this.error = 'Không thể tải thông tin văn hóa. Vui lòng thử lại sau.';
        this.loading = false;
        console.error('Error loading cultures:', err);
      }
    });
  }

  private async initAnimations(): Promise<void> {
    try {
      const { gsap } = await import('gsap');
      const { ScrollTrigger } = await import('gsap/ScrollTrigger');

      gsap.registerPlugin(ScrollTrigger);

      gsap.utils.toArray<HTMLElement>('.reveal-section').forEach(section => {
        gsap.from(section, {
          scrollTrigger: {
            trigger: section,
            start: 'top 85%',
            toggleActions: 'play none none none'
          },
          opacity: 0,
          y: 50,
          duration: 1,
          ease: 'power2.out'
        });
      });
    } catch (error) {
      console.error('Animation initialization failed:', error);
    }
  }
}
