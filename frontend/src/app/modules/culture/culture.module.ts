import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CultureComponent } from './culture.component';
import { SharedModule } from '../../shared/shared-module';

const routes: Routes = [
  {
    path: '',
    component: CultureComponent
  }
];

@NgModule({
  declarations: [
    CultureComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class CultureModule { }
