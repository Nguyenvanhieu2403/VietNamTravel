import { Component, OnInit, PLATFORM_ID, Inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

interface Destination {
  id: string;
  name: string;
  slug: string;
  category: string;
  description: string;
  imageUrl: string;
  season: string;
  budget: string;
  tags: string[];
  featured: boolean;
}

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

  public categories = [
    { id: 'all', name: 'Tất Cả', icon: '🌏' },
    { id: 'nature', name: 'Thiên Nhiên', icon: '🏞️' },
    { id: 'heritage', name: 'Di Sản', icon: '🏛️' },
    { id: 'beach', name: 'Biển Đảo', icon: '🏖️' },
    { id: 'mountain', name: 'Núi Rừng', icon: '⛰️' },
    { id: 'city', name: 'Thành Phố', icon: '🏙️' }
  ];

  public destinations: Destination[] = [
    {
      id: '1',
      name: 'Vịnh Hạ Long',
      slug: 'vinh-ha-long',
      category: 'nature',
      description: 'Di sản thiên nhiên thế giới với hàng ngàn hòn đảo đá vôi kỳ vĩ',
      imageUrl: 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=1200&q=80',
      season: 'Tháng 3 - Tháng 5',
      budget: '3-5 triệu VND',
      tags: ['UNESCO', 'Du thuyền', 'Hang động'],
      featured: true
    },
    {
      id: '2',
      name: 'Phố Cổ Hội An',
      slug: 'hoi-an',
      category: 'heritage',
      description: 'Thành phố cổ kính với kiến trúc độc đáo và đèn lồng rực rỡ',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=1200&q=80',
      season: 'Tháng 2 - Tháng 4',
      budget: '2-4 triệu VND',
      tags: ['UNESCO', 'Văn hóa', 'Ẩm thực'],
      featured: true
    },
    {
      id: '3',
      name: 'Ruộng Bậc Thang Sa Pa',
      slug: 'sapa',
      category: 'mountain',
      description: 'Thửa ruộng bậc thang uốn lượn như tranh vẽ giữa núi rừng Tây Bắc',
      imageUrl: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1200&q=80',
      season: 'Tháng 9 - Tháng 11',
      budget: '3-6 triệu VND',
      tags: ['Trekking', 'Văn hóa', 'Nhiếp ảnh'],
      featured: true
    },
    {
      id: '4',
      name: 'Đảo Phú Quốc',
      slug: 'phu-quoc',
      category: 'beach',
      description: 'Đảo ngọc với bãi biển cát trắng và làn nước trong xanh như pha lê',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=1200&q=80',
      season: 'Tháng 11 - Tháng 3',
      budget: '5-8 triệu VND',
      tags: ['Nghỉ dưỡng', 'Lặn biển', 'Hải sản'],
      featured: false
    },
    {
      id: '5',
      name: 'Động Phong Nha',
      slug: 'phong-nha',
      category: 'nature',
      description: 'Hệ thống hang động kỳ vĩ với những khối thạch nhũ tuyệt đẹp',
      imageUrl: 'https://images.unsplash.com/photo-1599708153386-62e2531a5ebf?auto=format&fit=crop&w=1200&q=80',
      season: 'Tháng 2 - Tháng 8',
      budget: '4-7 triệu VND',
      tags: ['UNESCO', 'Khám phá', 'Mạo hiểm'],
      featured: false
    },
    {
      id: '6',
      name: 'Thành Phố Hồ Chí Minh',
      slug: 'ho-chi-minh',
      category: 'city',
      description: 'Thành phố năng động với sự pha trộn giữa hiện đại và truyền thống',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=1200&q=80',
      season: 'Quanh năm',
      budget: '3-5 triệu VND',
      tags: ['Đô thị', 'Ẩm thực', 'Mua sắm'],
      featured: false
    }
  ];

  constructor(@Inject(PLATFORM_ID) platformId: Object) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    if (this.isBrowser) {
      this.initAnimations();
    }
  }

  get filteredDestinations(): Destination[] {
    return this.destinations.filter(dest => {
      const matchesCategory = this.selectedCategory() === 'all' || dest.category === this.selectedCategory();
      const matchesSearch = dest.name.toLowerCase().includes(this.searchQuery().toLowerCase());
      return matchesCategory && matchesSearch;
    });
  }

  get featuredDestinations(): Destination[] {
    return this.destinations.filter(dest => dest.featured);
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
