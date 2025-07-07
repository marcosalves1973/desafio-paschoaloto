import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'cadastro',
    pathMatch: 'full'
  },
  {
    path: 'cadastro',
    loadComponent: () =>
      import('./pages/cadastro/formulario-devedor.component').then(
        m => m.FormularioDevedorComponent
      )
  },
  {
    path: 'titulos',
    loadComponent: () =>
      import('./pages/titulos/titulos.component').then(
        m => m.TitulosComponent
      )
  }
];
