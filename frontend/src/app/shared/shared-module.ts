import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { LazyImageDirective } from './directives/lazy-image.directive';
import { SafeUrlPipe } from './pipes/safe-url.pipe';
import { NavbarComponent } from './components/navbar/navbar';
import { GlassPanelComponent } from './components/glass-panel/glass-panel';
import { SectionHeaderComponent } from './components/section-header/section-header';
import { CinematicCardComponent } from './components/cinematic-card/cinematic-card';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner';

@NgModule({
  declarations: [
    LazyImageDirective,
    SafeUrlPipe,
    NavbarComponent,
    GlassPanelComponent,
    SectionHeaderComponent,
    CinematicCardComponent,
    LoadingSpinnerComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    LazyImageDirective,
    SafeUrlPipe,
    NavbarComponent,
    GlassPanelComponent,
    SectionHeaderComponent,
    CinematicCardComponent,
    LoadingSpinnerComponent
  ]
})
export class SharedModule {}
