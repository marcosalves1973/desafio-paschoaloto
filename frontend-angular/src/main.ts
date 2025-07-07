import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideNgxMask } from 'ngx-mask';
import { FormularioDevedorComponent } from './app/pages/cadastro/formulario-devedor.component';
import { LOCALE_ID } from '@angular/core';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';
import { registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';


registerLocaleData(ptBr);

// bootstrapApplication(FormularioDevedorComponent, {
//   providers: [
    
//     provideHttpClient(),
//     provideAnimations(),
//     provideNgxMask(),
//     { provide: LOCALE_ID, useValue: 'pt-BR' }
//   ]
// });

bootstrapApplication(AppComponent, appConfig)
  .catch(err => console.error(err));
