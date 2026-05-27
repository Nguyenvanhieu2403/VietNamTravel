import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { DestinationsComponent } from './destinations.component';
import { SharedModule } from '../../shared/shared-module';

const routes: Routes = [
  {
    path: '',
    component: DestinationsComponent
  }
];

@NgModule({
  declarations: [
    DestinationsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class DestinationsModule { }
