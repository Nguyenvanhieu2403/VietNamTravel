import { Component, OnInit, Inject, PLATFORM_ID, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
  standalone: false
})
export class NavbarComponent implements OnInit {
  private isBrowser: boolean;
  public isScrolled = signal<boolean>(false);
  public isMobileMenuOpen = signal<boolean>(false);

  constructor(@Inject(PLATFORM_ID) platformId: Object) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    if (this.isBrowser) {
      window.addEventListener('scroll', () => {
        this.isScrolled.set(window.scrollY > 50);
      });
    }
  }

  toggleMobileMenu(): void {
    this.isMobileMenuOpen.update(val => !val);
  }
}
