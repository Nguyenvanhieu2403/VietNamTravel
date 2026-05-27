import { Component, OnInit, PLATFORM_ID, Inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

interface Article {
  id: string;
  title: string;
  slug: string;
  excerpt: string;
  category: string;
  imageUrl: string;
  author: string;
  date: string;
  readTime: string;
  featured: boolean;
}

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.scss'],
  standalone: false
})
export class BlogComponent implements OnInit {
  private isBrowser: boolean;

  public selectedCategory = signal<string>('all');
  public searchQuery = signal<string>('');

  public categories = [
    { id: 'all', name: 'Tất Cả' },
    { id: 'travel-guide', name: 'Cẩm Nang' },
    { id: 'culture', name: 'Văn Hóa' },
    { id: 'food', name: 'Ẩm Thực' },
    { id: 'adventure', name: 'Khám Phá' },
    { id: 'tips', name: 'Mẹo Du Lịch' }
  ];

  public articles: Article[] = [
    {
      id: '1',
      title: 'Hành Trình Khám Phá Vịnh Hạ Long: Thiên Đường Trên Biển',
      slug: 'halong-bay-journey',
      excerpt: 'Khám phá vẻ đẹp huyền bí của di sản thiên nhiên thế giới với hàng ngàn hòn đảo đá vôi kỳ vĩ',
      category: 'travel-guide',
      imageUrl: 'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=1200&q=80',
      author: 'Minh Anh',
      date: '15 Tháng 5, 2024',
      readTime: '8 phút đọc',
      featured: true
    },
    {
      id: '2',
      title: 'Ẩm Thực Đường Phố Sài Gòn: Hương Vị Miền Nam',
      slug: 'saigon-street-food',
      excerpt: 'Từ bánh mì thơm ngon đến phở nóng hổi, khám phá thiên đường ẩm thực đường phố Sài Gòn',
      category: 'food',
      imageUrl: 'https://images.unsplash.com/photo-1582878826629-29b7ad1cdc43?auto=format&fit=crop&w=1200&q=80',
      author: 'Thanh Hà',
      date: '12 Tháng 5, 2024',
      readTime: '6 phút đọc',
      featured: true
    },
    {
      id: '3',
      title: 'Sa Pa Mùa Lúa Chín: Bức Tranh Vàng Trên Núi',
      slug: 'sapa-rice-season',
      excerpt: 'Thời điểm đẹp nhất để chiêm ngưỡng ruộng bậc thang Sa Pa trong sắc vàng óng ánh',
      category: 'adventure',
      imageUrl: 'https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1200&q=80',
      author: 'Quốc Bảo',
      date: '10 Tháng 5, 2024',
      readTime: '7 phút đọc',
      featured: false
    },
    {
      id: '4',
      title: 'Phố Cổ Hội An: Ánh Đèn Lồng Lung Linh',
      slug: 'hoi-an-lanterns',
      excerpt: 'Trải nghiệm không gian cổ kính với hàng ngàn chiếc đèn lồng rực rỡ sắc màu',
      category: 'culture',
      imageUrl: 'https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&w=1200&q=80',
      author: 'Thu Hương',
      date: '8 Tháng 5, 2024',
      readTime: '5 phút đọc',
      featured: false
    },
    {
      id: '5',
      title: '10 Mẹo Du Lịch Việt Nam Tiết Kiệm Cho Backpacker',
      slug: 'budget-travel-tips',
      excerpt: 'Hướng dẫn chi tiết giúp bạn khám phá Việt Nam với ngân sách hợp lý',
      category: 'tips',
      imageUrl: 'https://images.unsplash.com/photo-1524230507669-5ff9e615b3e4?auto=format&fit=crop&w=1200&q=80',
      author: 'Đức Minh',
      date: '5 Tháng 5, 2024',
      readTime: '10 phút đọc',
      featured: false
    },
    {
      id: '6',
      title: 'Động Phong Nha: Hành Trình Vào Lòng Đất',
      slug: 'phong-nha-caves',
      excerpt: 'Khám phá hệ thống hang động kỳ vĩ với những khối thạch nhũ tuyệt đẹp',
      category: 'adventure',
      imageUrl: 'https://images.unsplash.com/photo-1599708153386-62e2531a5ebf?auto=format&fit=crop&w=1200&q=80',
      author: 'Hoàng Long',
      date: '3 Tháng 5, 2024',
      readTime: '9 phút đọc',
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

  get filteredArticles(): Article[] {
    return this.articles.filter(article => {
      const matchesCategory = this.selectedCategory() === 'all' || article.category === this.selectedCategory();
      const matchesSearch = article.title.toLowerCase().includes(this.searchQuery().toLowerCase()) ||
                           article.excerpt.toLowerCase().includes(this.searchQuery().toLowerCase());
      return matchesCategory && matchesSearch;
    });
  }

  get featuredArticle(): Article | undefined {
    return this.articles.find(article => article.featured);
  }

  get editorsPicks(): Article[] {
    return this.articles.filter(article => article.featured).slice(0, 2);
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

      gsap.from('.article-card', {
        scrollTrigger: {
          trigger: '.articles-grid',
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
