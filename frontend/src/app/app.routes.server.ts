import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: '',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'regions',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'provinces',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'provinces/:slug',
    renderMode: RenderMode.Server,
  },
  {
    path: '**',
    renderMode: RenderMode.Server,
  },
];
