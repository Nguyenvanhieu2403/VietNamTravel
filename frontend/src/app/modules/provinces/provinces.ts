import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

interface Province {
  id: string;
  name: string;
  slug: string;
  region: string;
  description: string;
  imageUrl: string;
  bestTimeToVisit: string;
  averageBudget: number;
}

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

  public provinces: Province[] = [
    {
      id: '1',
      name: 'Hà Nội',
      slug: 'ha-noi',
      region: 'mien-bac',
      description: 'Thủ đô ngàn năm văn hiến với Hồ Gươm thơ mộng, Văn Miếu cổ kính và phố cổ 36 phường phố tấp nập.',
      imageUrl: 'https://images.unsplash.com/photo-1509023464722-18d996393ca8?auto=format&fit=crop&w=800&q=80',
      bestTimeToVisit: 'Tháng 9 - Tháng 11',
      averageBudget: 3000000
    },
    {
      id: '2',
      name: 'Quảng Ninh',
      slug: 'quang-ninh',
      region: 'mien-bac',
      description: 'Vịnh Hạ Long kỳ vĩ với hàng ngàn hòn đảo đá vôi nhô lên giữa làn nước xanh ngọc bích.',
      imageUrl: 'https://images.unsplash.com/photo-1524230507669-5ff9e615b3e4?auto=format&fit=crop&w=800&q=80',
      bestTimeToVisit: 'Tháng 3 - Tháng 5',
      averageBudget: 5000000
    },
    {
      id: '3',
      name: 'Lào Cai',
      slug: 'lao-cai',
      region: 'mien-bac',
      description: 'Sa Pa với ruộng bậc thang hùng vĩ, sương mù bao phủ và văn hóa dân tộc thiểu số đặc sắc.',
      imageUrl: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=800&q=80',
      bestTimeToVisit: 'Tháng 9 - Tháng 11',
      averageBudget: 4000000
    },
    {
      id: '4',
      name: 'Đà Nẵng',
      slug: 'da-nang',
      region: 'mien-trung',
      description: 'Thành phố đáng sống với bãi biển Mỹ Khê tuyệt đẹp, cầu Rồng phun lửa và Bà Nà Hills huyền ảo.',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=800&q=80',
      bestTimeToVisit: 'Tháng 2 - Tháng 5',
      averageBudget: 4500000
    },
    {
      id: '5',
      name: 'Quảng Nam',
      slug: 'quang-nam',
      region: 'mien-trung',
      description: 'Hội An cổ kính với phố cổ lung linh đèn lồng, Mỹ Sơn linh thiêng và bãi biển An Bàng yên bình.',
      imageUrl: 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=800&q=80',
      bestTimeToVisit: 'Tháng 2 - Tháng 8',
      averageBudget: 4000000
    },
    {
      id: '6',
      name: 'Lâm Đồng',
      slug: 'lam-dong',
      region: 'tay-nguyen',
      description: 'Đà Lạt thành phố ngàn hoa với khí hậu mát mẻ quanh năm, hồ Xuân Hương thơ mộng và đồi chè xanh mướt.',
      imageUrl: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=800&q=80',
      bestTimeToVisit: 'Tháng 11 - Tháng 3',
      averageBudget: 3500000
    },
    {
      id: '7',
      name: 'TP. Hồ Chí Minh',
      slug: 'tp-ho-chi-minh',
      region: 'mien-nam',
      description: 'Thành phố năng động nhất Việt Nam với nhịp sống sôi động, ẩm thực phong phú và cuộc sống về đêm sầm uất.',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=800&q=80',
      bestTimeToVisit: 'Tháng 12 - Tháng 4',
      averageBudget: 5000000
    },
    {
      id: '8',
      name: 'Kiên Giang',
      slug: 'kien-giang',
      region: 'dong-bang-song-cuu-long',
      description: 'Phú Quốc đảo ngọc với bãi biển cát trắng mịn màng, làn nước trong xanh và rừng nhiệt đới nguyên sinh.',
      imageUrl: 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=800&q=80',
      bestTimeToVisit: 'Tháng 11 - Tháng 3',
      averageBudget: 6000000
    }
  ];

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private route: ActivatedRoute
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['region']) {
        this.selectedRegion = params['region'];
      }
    });

    if (this.isBrowser) {
      this.initAnimations();
    }
  }

  get filteredProvinces(): Province[] {
    return this.provinces.filter(province => {
      const matchesRegion = this.selectedRegion === 'all' || province.region === this.selectedRegion;
      const matchesSearch = province.name.toLowerCase().includes(this.searchQuery.toLowerCase());
      return matchesRegion && matchesSearch;
    });
  }

  filterByRegion(region: string): void {
    this.selectedRegion = region;
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
