import { Component, OnInit, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

interface CulturalRegion {
  id: string;
  name: string;
  description: string;
  imageUrl: string;
  highlights: string[];
}

interface Festival {
  id: string;
  name: string;
  month: string;
  description: string;
  imageUrl: string;
}

interface Cuisine {
  id: string;
  name: string;
  region: string;
  description: string;
  imageUrl: string;
}

@Component({
  selector: 'app-culture',
  templateUrl: './culture.component.html',
  styleUrls: ['./culture.component.scss'],
  standalone: false
})
export class CultureComponent implements OnInit {
  private isBrowser: boolean;

  public culturalRegions: CulturalRegion[] = [
    {
      id: '1',
      name: 'Miền Bắc',
      description: 'Nơi lưu giữ bản sắc văn hóa truyền thống với hơn 1000 năm lịch sử',
      imageUrl: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1200&q=80',
      highlights: ['Hát Quan họ', 'Lễ hội đền Hùng', 'Nghề thủ công truyền thống']
    },
    {
      id: '2',
      name: 'Miền Trung',
      description: 'Vùng đất di sản với kiến trúc cổ kính và văn hóa Chăm độc đáo',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=1200&q=80',
      highlights: ['Nhã nhạc cung đình Huế', 'Hội An đèn lồng', 'Tháp Chăm']
    },
    {
      id: '3',
      name: 'Miền Nam',
      description: 'Vùng đất phồn thịnh với văn hóa đa dạng và ẩm thực phong phú',
      imageUrl: 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=1200&q=80',
      highlights: ['Đờn ca tài tử', 'Chợ nổi', 'Văn hóa sông nước']
    }
  ];

  public festivals: Festival[] = [
    {
      id: '1',
      name: 'Tết Nguyên Đán',
      month: 'Tháng 1-2',
      description: 'Lễ hội truyền thống quan trọng nhất của người Việt',
      imageUrl: 'https://images.unsplash.com/photo-1508873696983-2df519f0397e?auto=format&fit=crop&w=800&q=80'
    },
    {
      id: '2',
      name: 'Lễ Hội Đèn Lồng Hội An',
      month: 'Hàng tháng',
      description: 'Phố cổ rực sáng với hàng ngàn chiếc đèn lồng',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=800&q=80'
    },
    {
      id: '3',
      name: 'Lễ Hội Hoa Ban',
      month: 'Tháng 3',
      description: 'Lễ hội của người dân tộc Tây Bắc',
      imageUrl: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=800&q=80'
    },
    {
      id: '4',
      name: 'Lễ Hội Đền Hùng',
      month: 'Tháng 3',
      description: 'Tưởng nhớ công đức các vua Hùng dựng nước',
      imageUrl: 'https://images.unsplash.com/photo-1524230507669-5ff9e615b3e4?auto=format&fit=crop&w=800&q=80'
    }
  ];

  public cuisines: Cuisine[] = [
    {
      id: '1',
      name: 'Phở',
      region: 'Miền Bắc',
      description: 'Món ăn quốc hồn quốc tuý với nước dùng thanh ngọt',
      imageUrl: 'https://images.unsplash.com/photo-1582878826629-29b7ad1cdc43?auto=format&fit=crop&w=800&q=80'
    },
    {
      id: '2',
      name: 'Bún Bò Huế',
      region: 'Miền Trung',
      description: 'Hương vị đậm đà đặc trưng của cố đô',
      imageUrl: 'https://images.unsplash.com/photo-1559314809-0d155014e29e?auto=format&fit=crop&w=800&q=80'
    },
    {
      id: '3',
      name: 'Bánh Xèo',
      region: 'Miền Nam',
      description: 'Bánh giòn rụm với nhân tôm thịt phong phú',
      imageUrl: 'https://images.unsplash.com/photo-1626804475297-41608ea09aeb?auto=format&fit=crop&w=800&q=80'
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
