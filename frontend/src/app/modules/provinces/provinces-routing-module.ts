import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProvincesComponent } from './provinces';
import { ProvinceDetailComponent } from './province-detail';

const routes: Routes = [
  {
    path: '',
    component: ProvincesComponent
  },
  {
    path: ':slug',
    component: ProvinceDetailComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProvincesRoutingModule {}
