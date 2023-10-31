import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import {  HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from "@angular/common/http";
import { importProvidersFrom } from '@angular/core';
import { AppRoutingModule } from './app/app.routes.module';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AuthInterceptor } from './app/core/interceptors/auth.interceptor';



bootstrapApplication(AppComponent, {
   providers: [
      provideHttpClient(
         withInterceptorsFromDi(),
     ),
      importProvidersFrom(AppRoutingModule),
      {
          provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true 
      }
     
   ],
}).then(ref => {
   // Ensure Angular destroys itself on hot reloads.
   if (window['ngRef']) {
     window['ngRef'].destroy();
   }
   window['ngRef'] = ref;}).catch((err) => console.error(err));

  