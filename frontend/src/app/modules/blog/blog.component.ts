import { Component, OnInit, PLATFORM_ID, Inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BlogService } from '../../core/services/blog.service';
import { BlogDto } from '../../core/models/travel.models';

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
  public loading = signal<boolean>(true);
  public error = signal<string | null>(null);

  public categories = [
    { id: 'all', name: 'Tất Cả' },
    { id: 'travel-guide', name: 'Cẩm Nang' },
    { id: 'culture', name: 'Văn Hóa' },
    { id: 'food', name: 'Ẩm Thực' },
    { id: 'adventure', name: 'Khám Phá' },
    { id: 'tips', name: 'Mẹo Du Lịch' }
  ];

  public articles: BlogDto[] = [];
  public allArticles: BlogDto[] = [];

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private blogService: BlogService
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.loadBlogs();
  }

  private loadBlogs(): void {
    this.loading.set(true);
    this.error.set(null);

    this.blogService.getBlogs(1, 50).subscribe({
      next: (data) => {
        this.allArticles = data.items;
        this.articles = data.items;
        this.loading.set(false);
        if (this.isBrowser) {
          setTimeout(() => this.initAnimations(), 100);
        }
      },
      error: (err) => {
        this.error.set('Không thể tải danh sách bài viết. Vui lòng thử lại sau.');
        this.loading.set(false);
        console.error('Error loading blogs:', err);
      }
    });
  }

  get filteredArticles(): BlogDto[] {
    return this.allArticles.filter(article => {
      const matchesCategory = this.selectedCategory() === 'all' ||
                             (article.tags && article.tags.toLowerCase().includes(this.selectedCategory()));
      const matchesSearch = article.title.toLowerCase().includes(this.searchQuery().toLowerCase()) ||
                           (article.summary && article.summary.toLowerCase().includes(this.searchQuery().toLowerCase()));
      return matchesCategory && matchesSearch;
    });
  }

  get featuredArticle(): BlogDto | undefined {
    return this.allArticles.find(article => article.isFeatured);
  }

  get editorsPicks(): BlogDto[] {
    return this.allArticles.filter(article => article.isFeatured).slice(0, 2);
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
