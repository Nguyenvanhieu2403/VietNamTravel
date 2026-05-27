import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared-module';
import { ProvincesRoutingModule } from './provinces-routing-module';
import { ProvincesComponent } from './provinces';
import { ProvinceDetailComponent } from './province-detail';

@NgModule({
  declarations: [ProvincesComponent, ProvinceDetailComponent],
  imports: [SharedModule, ProvincesRoutingModule]
})
export class ProvincesModule {}
