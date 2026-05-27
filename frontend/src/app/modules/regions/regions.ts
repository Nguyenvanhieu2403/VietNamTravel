import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

interface Region {
  id: string;
  name: string;
  slug: string;
  description: string;
  imageUrl: string;
  provinces: string[];
  highlights: string[];
  bestSeason: string;
  color: string;
}

@Component({
  selector: 'app-regions',
  templateUrl: './regions.html',
  styleUrl: './regions.scss',
  standalone: false
})
export class RegionsComponent implements OnInit {
  private isBrowser: boolean;

  public regions: Region[] = [
    {
      id: '1',
      name: 'Miền Bắc',
      slug: 'mien-bac',
      description: 'Vùng đất ngàn năm văn hiến với Thủ đô Hà Nội cổ kính, ruộng bậc thang Sapa hùng vĩ, Vịnh Hạ Long huyền bí và cao nguyên đá Hà Giang hoang sơ.',
      imageUrl: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1200&q=80',
      provinces: ['Hà Nội', 'Hải Phòng', 'Quảng Ninh', 'Lào Cai', 'Hà Giang', 'Ninh Bình'],
      highlights: ['Vịnh Hạ Long', 'Sa Pa', 'Hà Giang', 'Tràng An', 'Hồ Ba Bể'],
      bestSeason: 'Tháng 9 - Tháng 11',
      color: '#D4AF37'
    },
    {
      id: '2',
      name: 'Miền Trung',
      slug: 'mien-trung',
      description: 'Dải đất di sản với Cố đô Huế trầm mặc, phố cổ Hội An lung linh đèn lồng, Đà Nẵng hiện đại và bãi biển Nha Trang tuyệt đẹp.',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=1200&q=80',
      provinces: ['Đà Nẵng', 'Huế', 'Quảng Nam', 'Khánh Hòa', 'Quảng Bình', 'Phú Yên'],
      highlights: ['Hội An', 'Huế', 'Bà Nà Hills', 'Nha Trang', 'Động Phong Nha'],
      bestSeason: 'Tháng 2 - Tháng 8',
      color: '#0F4C3A'
    },
    {
      id: '3',
      name: 'Tây Nguyên',
      slug: 'tay-nguyen',
      description: 'Cao nguyên bạt ngàn với đồi chè xanh mướt, hương cà phê đặc trưng, thác nước hùng vĩ và Đà Lạt sương mù thơ mộng.',
      imageUrl: 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=1200&q=80',
      provinces: ['Đà Lạt', 'Buôn Ma Thuột', 'Pleiku', 'Kon Tum'],
      highlights: ['Đà Lạt', 'Măng Đen', 'Hồ Tà Đùng', 'Thác Dray Nur', 'Làng Cà Phê'],
      bestSeason: 'Tháng 11 - Tháng 4',
      color: '#A0522D'
    },
    {
      id: '4',
      name: 'Miền Nam',
      slug: 'mien-nam',
      description: 'Trung tâm kinh tế sôi động với TP.HCM không ngủ, Vũng Tàu tràn ngập nắng ấm, Côn Đảo hoang sơ và Tây Ninh linh thiêng.',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=1200&q=80',
      provinces: ['TP. Hồ Chí Minh', 'Vũng Tàu', 'Côn Đảo', 'Tây Ninh', 'Bình Dương'],
      highlights: ['Sài Gòn', 'Vũng Tàu', 'Côn Đảo', 'Núi Bà Đen', 'Địa Đạo Củ Chi'],
      bestSeason: 'Tháng 12 - Tháng 4',
      color: '#4682B4'
    },
    {
      id: '5',
      name: 'Đồng Bằng Sông Cửu Long',
      slug: 'dong-bang-song-cuu-long',
      description: 'Vùng sông nước bình dị với chợ nổi Cái Răng tấp nập, rừng tràm Trà Sư xanh thẳm, xứ dừa Bến Tre và đảo ngọc Phú Quốc.',
      imageUrl: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1200&q=80',
      provinces: ['Cần Thơ', 'An Giang', 'Phú Quốc', 'Bến Tre', 'Cà Mau', 'Sóc Trăng'],
      highlights: ['Chợ Nổi Cái Răng', 'Phú Quốc', 'Rừng Tràm Trà Sư', 'Bến Tre', 'Mũi Cà Mau'],
      bestSeason: 'Tháng 12 - Tháng 5',
      color: '#2E8B57'
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
