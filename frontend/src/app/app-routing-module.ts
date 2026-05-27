import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./modules/home/home-module').then(m => m.HomeModule)
  },
  {
    path: 'regions',
    loadChildren: () => import('./modules/regions/regions-module').then(m => m.RegionsModule)
  },
  {
    path: 'provinces',
    loadChildren: () => import('./modules/provinces/provinces-module').then(m => m.ProvincesModule)
  },
  {
    path: 'destinations',
    loadChildren: () => import('./modules/destinations/destinations.module').then(m => m.DestinationsModule)
  },
  {
    path: 'culture',
    loadChildren: () => import('./modules/culture/culture.module').then(m => m.CultureModule)
  },
  {
    path: 'blog',
    loadChildren: () => import('./modules/blog/blog.module').then(m => m.BlogModule)
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
