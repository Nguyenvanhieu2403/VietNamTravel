import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-province-detail',
  templateUrl: './province-detail.html',
  styleUrl: './province-detail.scss',
  standalone: false
})
export class ProvinceDetailComponent implements OnInit {
  private isBrowser: boolean;
  public provinceSlug: string = '';

  // Mock data - in real app, fetch from API
  public province = {
    name: 'Quảng Ninh',
    region: 'Miền Bắc',
    description: 'Quảng Ninh là tỉnh ven biển phía Đông Bắc Việt Nam, nổi tiếng với Vịnh Hạ Long - Di sản Thiên nhiên Thế giới được UNESCO công nhận. Nơi đây hội tụ vẻ đẹp hùng vĩ của núi non, biển cả và hàng ngàn hòn đảo đá vôi kỳ vĩ.',
    heroImage: 'https://images.unsplash.com/photo-1524230507669-5ff9e615b3e4?auto=format&fit=crop&w=1600&q=80',
    gallery: [
      'https://images.unsplash.com/photo-1524230507669-5ff9e615b3e4?auto=format&fit=crop&w=800&q=80',
      'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=800&q=80',
      'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=800&q=80',
      'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=800&q=80'
    ],
    highlights: [
      { name: 'Vịnh Hạ Long', description: 'Di sản thiên nhiên thế giới với hàng ngàn hòn đảo đá vôi' },
      { name: 'Đảo Cô Tô', description: 'Hòn đảo hoang sơ với bãi biển cát trắng tuyệt đẹp' },
      { name: 'Yên Tử', description: 'Quần thể danh thắng Phật giáo linh thiêng trên núi cao' },
      { name: 'Bãi Cháy', description: 'Bãi biển nhân tạo hiện đại với nhiều tiện ích du lịch' }
    ],
    foods: [
      { name: 'Chả mực Hạ Long', image: 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=400&q=80' },
      { name: 'Ngán Hạ Long', image: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=400&q=80' },
      { name: 'Sò huyết nướng', image: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=400&q=80' },
      { name: 'Bánh gật gù', image: 'https://images.unsplash.com/photo-1524230507669-5ff9e615b3e4?auto=format&fit=crop&w=400&q=80' }
    ],
    bestTimeToVisit: 'Tháng 3 - Tháng 5, Tháng 9 - Tháng 11',
    averageBudget: '5.000.000 - 10.000.000 VND',
    weather: [
      { month: 'T1-T3', temp: '15-20°C', condition: 'Lạnh, sương mù' },
      { month: 'T4-T6', temp: '25-30°C', condition: 'Ấm áp, nắng đẹp' },
      { month: 'T7-T9', temp: '28-35°C', condition: 'Nóng, mưa dông' },
      { month: 'T10-T12', temp: '20-25°C', condition: 'Mát mẻ, khô ráo' }
    ]
  };

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private route: ActivatedRoute
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.provinceSlug = params['slug'];
      // In real app: fetch province data from API using slug
    });

    if (this.isBrowser) {
      this.initAnimations();
    }
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
        ease: 'power3.out'
      });

      gsap.utils.toArray<HTMLElement>('.fade-in-section').forEach(section => {
        gsap.from(section, {
          scrollTrigger: {
            trigger: section,
            start: 'top 80%',
            toggleActions: 'play none none none'
          },
          y: 60,
          opacity: 0,
          duration: 0.8,
          ease: 'power2.out'
        });
      });
    } catch (err) {
      console.warn('Could not load GSAP: ', err);
    }
  }
}
