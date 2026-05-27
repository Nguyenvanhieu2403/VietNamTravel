import { Component, OnInit, Inject, PLATFORM_ID, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ApiService } from '../../core/services/api.service';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrl: './home.scss',
  standalone: false
})
export class HomeComponent implements OnInit {
  private isBrowser: boolean;

  // Active region state for interactive SVG map
  public activeRegion = signal<any>(null);

  // Region static previews
  public regionsData: { [key: string]: any } = {
    'mien-bac': {
      name: 'Miền Bắc',
      description: 'Vùng đất ngàn năm văn hiến, hùng vĩ với ruộng bậc thang Sapa, Vịnh Hạ Long huyền bí và thủ đô Hà Nội cổ kính.',
      highlights: ['Hà Nội', 'Sa Pa', 'Vịnh Hạ Long', 'Hà Giang'],
      color: '#D4AF37'
    },
    'mien-trung': {
      name: 'Miền Trung',
      description: 'Dải đất di sản hội tụ các bãi biển cát trắng nắng vàng tuyệt đẹp, Cố đô Huế trầm mặc và phố cổ Hội An lung linh.',
      highlights: ['Đà Nẵng', 'Hội An', 'Huế', 'Nha Trang'],
      color: '#0F4C3A'
    },
    'tay-nguyen': {
      name: 'Tây Nguyên',
      description: 'Vùng cao nguyên bạt ngàn ngập tràn hương cà phê, tiếng cồng chiêng vang vọng, thác nước hùng vĩ và Đà Lạt sương mù.',
      highlights: ['Đà Lạt', 'Buôn Ma Thuột', 'Măng Đen', 'Hồ Tà Đùng'],
      color: '#A0522D'
    },
    'mien-nam': {
      name: 'Miền Nam',
      description: 'Trung tâm kinh tế năng động bậc nhất Việt Nam với TP.HCM không ngủ, Vũng Tàu tràn ngập nắng ấm và Tây Ninh linh thiêng.',
      highlights: ['TP. Hồ Chí Minh', 'Vũng Tàu', 'Côn Đảo', 'Tây Ninh'],
      color: '#4682B4'
    },
    'dong-bang-song-cuu-long': {
      name: 'Mekong Delta',
      description: 'Vùng sông nước bình dị, chợ nổi Cái Răng tấp nập thuyền hoa, rừng tràm Trà Sư xanh thẳm và xứ dừa Bến Tre thơ mộng.',
      highlights: ['Cần Thơ', 'An Giang', 'Phú Quốc', 'Bến Tre'],
      color: '#2E8B57'
    }
  };

  // AI Recommendation engine state
  public aiBudget = signal<number>(5); // Default 5 million
  public aiStyle = signal<string>('Cultural');
  public aiGroup = signal<string>('Couple');
  public aiMonth = signal<number>(new Date().getMonth() + 1);
  public loadingAi = signal<boolean>(false);
  public aiRecommendationResult = signal<any>(null);

  constructor(
    private apiService: ApiService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    // Select Northern Vietnam by default
    this.activeRegion.set(this.regionsData['mien-bac']);

    if (this.isBrowser) {
      this.initGsapAnimations();
    }
  }

  // Hover SVG Region handler
  onRegionHover(regionId: string): void {
    if (this.regionsData[regionId]) {
      this.activeRegion.set(this.regionsData[regionId]);
    }
  }

  // Request AI Recommendations from Web API
  getAIRecommendation(): void {
    this.loadingAi.set(true);
    
    let params = new HttpParams()
      .set('budget', (this.aiBudget() * 1000000).toString()) // convert millions to VND
      .set('travelStyle', this.aiStyle())
      .set('groupType', this.aiGroup())
      .set('month', this.aiMonth().toString());

    this.apiService.get<any>('AIRecommendations', params).subscribe({
      next: (res) => {
        this.aiRecommendationResult.set(res);
        this.loadingAi.set(false);
        
        // Scroll to results using GSAP
        if (this.isBrowser) {
          setTimeout(() => {
            const resultSection = document.getElementById('ai-results');
            if (resultSection) {
              resultSection.scrollIntoView({ behavior: 'smooth' });
            }
          }, 100);
        }
      },
      error: (err) => {
        console.error(err);
        this.loadingAi.set(false);
      }
    });
  }

  // GSAP animations initializer
  private async initGsapAnimations(): Promise<void> {
    try {
      const { gsap } = await import('gsap');
      const { ScrollTrigger } = await import('gsap/ScrollTrigger');

      gsap.registerPlugin(ScrollTrigger);

      // Hero content entrance reveal
      gsap.from('.hero-content h1', {
        y: 60,
        opacity: 0,
        duration: 1.2,
        ease: 'power3.out'
      });

      gsap.from('.hero-content p', {
        y: 30,
        opacity: 0,
        duration: 1.2,
        delay: 0.3,
        ease: 'power3.out'
      });

      gsap.from('.hero-content .cta-group', {
        y: 30,
        opacity: 0,
        duration: 1.2,
        delay: 0.5,
        ease: 'power3.out'
      });

      // Scroll reveal animations for sections
      gsap.utils.toArray<HTMLElement>('.reveal-section').forEach(section => {
        gsap.from(section, {
          scrollTrigger: {
            trigger: section,
            start: 'top 80%',
            toggleActions: 'play none none none'
          },
          y: 50,
          opacity: 0,
          duration: 1,
          ease: 'power2.out'
        });
      });

      // Animated counter for statistics
      gsap.utils.toArray<HTMLElement>('.stat-number').forEach(stat => {
        const targetValue = parseInt(stat.getAttribute('data-target') || '0');
        gsap.from(stat, {
          scrollTrigger: {
            trigger: stat,
            start: 'top 85%',
            toggleActions: 'play none none none'
          },
          textContent: 0,
          duration: 2,
          ease: 'power1.out',
          snap: { textContent: 1 },
          onUpdate: function() {
            const current = parseFloat(stat.textContent || '0');
            stat.textContent = Math.ceil(current).toString();
          }
        });
      });

      // Parallax effect for cards
      gsap.utils.toArray<HTMLElement>('.parallax-card').forEach(card => {
        gsap.to(card, {
          scrollTrigger: {
            trigger: card,
            start: 'top bottom',
            end: 'bottom top',
            scrub: 1
          },
          y: -50,
          ease: 'none'
        });
      });
    } catch (err) {
      console.warn('Could not load GSAP: ', err);
    }
  }
}
