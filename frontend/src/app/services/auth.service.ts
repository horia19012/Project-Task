import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../models/loginRequest';
import { RegisterRequest } from '../models/registerRequest';

const baseUrl = 'http://localhost:5249/api/auth';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) {
  }
  login(loginRequest: LoginRequest) {
    return this.http.post(`${baseUrl}/login`, loginRequest);
  }

  register(registerRequest: RegisterRequest) {
    return this.http.post(`${baseUrl}/register`, registerRequest);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserIdFromToken(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return parseInt(payload.sub, 10);
    } catch {
      return null;
    }
  }
}
