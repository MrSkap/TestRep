import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceHealth } from './service-health';
@Injectable({
  providedIn: 'root'
})
export class HealthServiceService {
readonly APIUrl = "http://localhost:5000/api/health"
  constructor(private http:HttpClient) { }

  getServiceHealth():Observable<ServiceHealth[]>{

    return this.http.get<ServiceHealth[]>(this.APIUrl);
  }
}

