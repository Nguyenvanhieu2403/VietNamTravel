import { Directive, ElementRef, Input, OnInit, OnDestroy, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Directive({
  selector: 'img[appLazyImage]',
  standalone: false
})
export class LazyImageDirective implements OnInit, OnDestroy {
  @Input('appLazyImage') lazySrc!: string;
  private observer!: IntersectionObserver;
  private isBrowser: boolean;

  constructor(
    private el: ElementRef<HTMLImageElement>,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    if (!this.isBrowser) {
      // Fallback for SSR: Load directly or leave empty for client hydration
      this.el.nativeElement.src = this.lazySrc;
      return;
    }

    if ('IntersectionObserver' in window) {
      this.observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
          if (entry.isIntersecting) {
            this.loadImage();
            this.observer.unobserve(this.el.nativeElement);
          }
        });
      }, {
        rootMargin: '100px' // Load slightly before coming into viewport
      });

      this.observer.observe(this.el.nativeElement);
    } else {
      // Fallback for older browsers
      this.loadImage();
    }
  }

  private loadImage(): void {
    this.el.nativeElement.src = this.lazySrc;
  }

  ngOnDestroy(): void {
    if (this.observer) {
      this.observer.disconnect();
    }
  }
}
