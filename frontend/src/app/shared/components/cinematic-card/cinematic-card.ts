import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-cinematic-card',
  templateUrl: './cinematic-card.html',
  styleUrl: './cinematic-card.scss',
  standalone: false
})
export class CinematicCardComponent {
  @Input() imageUrl: string = '';
  @Input() title: string = '';
  @Input() description: string = '';
  @Input() tag: string = '';
  @Input() link: string = '';
  @Input() aspectRatio: string = '16/9';
}
