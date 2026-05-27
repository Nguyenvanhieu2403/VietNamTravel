import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.html',
  styleUrl: './section-header.scss',
  standalone: false
})
export class SectionHeaderComponent {
  @Input() tag: string = '';
  @Input() title: string = '';
  @Input() description: string = '';
  @Input() align: 'left' | 'center' = 'center';
}
