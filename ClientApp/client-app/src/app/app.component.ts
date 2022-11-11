import { Component, OnInit } from '@angular/core';
import { HealthServiceService } from './health-service.service';
import { ServiceHealth } from './service-health'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent{
  title = 'client-app';
}
