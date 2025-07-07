import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideNgxMask } from 'ngx-mask';
import { LOCALE_ID } from '@angular/core';



export const appConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(), // 👈 ESSENCIAL para HttpClient funcionar com standalone
    provideAnimations(), // 👈 ESSENCIAL para Material funcionar
    provideNgxMask(),
    { provide: LOCALE_ID, useValue: 'pt-BR' }

  ]
};