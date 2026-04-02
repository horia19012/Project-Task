import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Device } from '../models/device';

const baseUrl = 'http://localhost:5249/api/device';

@Injectable({
  providedIn: 'root',
})
export class DeviceService {

  token : string | null = localStorage.getItem('token');
  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<Device[]> {
    return this.httpClient.get<Device[]>(baseUrl);
  }
  getDevice(id: number): Observable<Device> {
    return this.httpClient.get<Device>(`${baseUrl}/${id}`);
  }
  create(data: Device): Observable<Device> {
    return this.httpClient.post<Device>(baseUrl, data);
  }
  update(id: number, data: Device): Observable<Device> {
    return this.httpClient.put<Device>(`${baseUrl}/${id}`, data);
  }
  delete(id: number): Observable<any> {
    return this.httpClient.delete(`${baseUrl}/${id}`);
  }
}
