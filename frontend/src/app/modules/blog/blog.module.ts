import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BlogComponent } from './blog.component';
import { SharedModule } from '../../shared/shared-module';

const routes: Routes = [
  {
    path: '',
    component: BlogComponent
  }
];

@NgModule({
  declarations: [
    BlogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class BlogModule { }
