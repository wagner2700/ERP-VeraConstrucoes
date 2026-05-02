import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IDashboardSummary } from '../models/interfaces/dashboard/IDashboardSummary.interface';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = '/api/dashboard';

  constructor(private http: HttpClient) {}

  getDashboardSummary(): Observable<IDashboardSummary> {
    return this.http.get<IDashboardSummary>(`${this.apiUrl}/summary`);
  }
}