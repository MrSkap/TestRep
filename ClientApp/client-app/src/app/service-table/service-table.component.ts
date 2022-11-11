import { Component, OnInit } from '@angular/core';
import { HealthServiceService } from '../health-service.service';
import { ServiceHealth } from '../service-health'
@Component({
  selector: 'app-service-table',
  templateUrl: './service-table.component.html',
  styleUrls: ['./service-table.component.css']
})
export class ServiceTableComponent implements OnInit {

  constructor(private service: HealthServiceService){}

  ngOnInit(): void {
    this.refreshServicesList();
  }
  ServicesList:ServiceHealth[]=[];

  refreshServicesList(){
    this.service.getServiceHealth().subscribe(data =>{
      this.ServicesList = data;
    });
  }
}
