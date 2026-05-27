import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-glass-panel',
  templateUrl: './glass-panel.html',
  styleUrl: './glass-panel.scss',
  standalone: false
})
export class GlassPanelComponent {
  @Input() borderColor: string = '';
  @Input() padding: string = '40px';
}
