import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

const baseUrl = 'http://localhost:5249/api/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) { }
  getAll() {
    return this.http.get(baseUrl);
  }
  getUser(id: number) {
    return this.http.get(`${baseUrl}/${id}`);
  }
  create(data: any) {
    return this.http.post(baseUrl, data);
  }
  update(id: number, data: any) {
    return this.http.put(`${baseUrl}/${id}`, data);
  }
  delete(id: number) {
    return this.http.delete(`${baseUrl}/${id}`);
  }

}
