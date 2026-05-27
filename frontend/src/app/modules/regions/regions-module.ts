import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared-module';
import { RegionsRoutingModule } from './regions-routing-module';
import { RegionsComponent } from './regions';

@NgModule({
  declarations: [RegionsComponent],
  imports: [SharedModule, RegionsRoutingModule]
})
export class RegionsModule {}
