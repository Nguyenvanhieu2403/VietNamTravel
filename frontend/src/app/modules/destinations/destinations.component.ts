import { Component, OnInit, PLATFORM_ID, Inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { DestinationService } from '../../core/services/destination.service';
import { DestinationDto } from '../../core/models/travel.models';

@Component({
  selector: 'app-destinations',
  templateUrl: './destinations.component.html',
  styleUrls: ['./destinations.component.scss'],
  standalone: false
})
export class DestinationsComponent implements OnInit {
  private isBrowser: boolean;

  public selectedCategory = signal<string>('all');
  public searchQuery = signal<string>('');
  public loading = signal<boolean>(true);
  public error = signal<string | null>(null);

  public categories = [
    { id: 'all', name: 'Tất Cả', icon: '🌏' },
    { id: 'nature', name: 'Thiên Nhiên', icon: '🏞️' },
    { id: 'heritage', name: 'Di Sản', icon: '🏛️' },
    { id: 'beach', name: 'Biển Đảo', icon: '🏖️' },
    { id: 'mountain', name: 'Núi Rừng', icon: '⛰️' },
    { id: 'city', name: 'Thành Phố', icon: '🏙️' }
  ];

  public destinations: DestinationDto[] = [];
  public allDestinations: DestinationDto[] = [];

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private destinationService: DestinationService
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.loadDestinations();
  }

  private loadDestinations(): void {
    this.loading.set(true);
    this.error.set(null);

    this.destinationService.getDestinations(1, 50).subscribe({
      next: (data) => {
        this.allDestinations = data.items;
        this.destinations = data.items;
        this.loading.set(false);
        if (this.isBrowser) {
          setTimeout(() => this.initAnimations(), 100);
        }
      },
      error: (err) => {
        this.error.set('Không thể tải danh sách điểm đến. Vui lòng thử lại sau.');
        this.loading.set(false);
        console.error('Error loading destinations:', err);
      }
    });
  }

  get filteredDestinations(): DestinationDto[] {
    return this.allDestinations.filter(dest => {
      const matchesCategory = this.selectedCategory() === 'all' || dest.category === this.selectedCategory();
      const matchesSearch = dest.name.toLowerCase().includes(this.searchQuery().toLowerCase()) ||
                           (dest.description && dest.description.toLowerCase().includes(this.searchQuery().toLowerCase()));
      return matchesCategory && matchesSearch;
    });
  }

  get featuredDestinations(): DestinationDto[] {
    return this.allDestinations.filter(dest => dest.isFeatured);
  }

  selectCategory(categoryId: string): void {
    this.selectedCategory.set(categoryId);
  }

  onSearchChange(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  private async initAnimations(): Promise<void> {
    try {
      const { gsap } = await import('gsap');
      const { ScrollTrigger } = await import('gsap/ScrollTrigger');

      gsap.registerPlugin(ScrollTrigger);

      // Reveal sections on scroll
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

      // Stagger destination cards
      gsap.from('.destination-card', {
        scrollTrigger: {
          trigger: '.destinations-grid',
          start: 'top 80%'
        },
        opacity: 0,
        y: 30,
        stagger: 0.1,
        duration: 0.8,
        ease: 'power2.out'
      });
    } catch (error) {
      console.error('Animation initialization failed:', error);
    }
  }
}
