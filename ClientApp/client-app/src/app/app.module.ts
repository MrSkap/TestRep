import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HealthServiceService } from './health-service.service';

import { HttpClientModule } from '@angular/common/http';
import { ServiceTableComponent } from './service-table/service-table.component'

@NgModule({
  declarations: [
    AppComponent,
    ServiceTableComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [HealthServiceService],
  bootstrap: [AppComponent]
})
export class AppModule {}
