import { ChangeDetectorRef, Component, inject, signal } from '@angular/core';
import { UserService } from './services/user.service';
import { AuthService } from './services/auth.service';
import {Router} from "@angular/router";
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');
  router = inject(Router);
  profileOpen = false;
  profileData: any = null;
  editMode = false;
  editName = '';
  editLocation = '';
  saving = false;
  userId: number | null = null;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
  ) {}

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  toggleProfile(): void {
    this.profileOpen = !this.profileOpen;
    if (this.profileOpen && !this.profileData) {
      this.loadProfile();
    }
  }

  logOut(): void {
    localStorage.removeItem('token');
    this.profileData = null;
    this.profileOpen = false;
    this.editMode = false;

    this.router.navigate(['/login']);
    
  }

  closeProfile(): void {
    this.profileOpen = false;
    this.editMode = false;
  }

  loadProfile(): void {
    const userId = this.authService.getUserIdFromToken();
    if (userId) {
      this.userService.getUser(userId).subscribe({
        next: (user: any) => {
          this.profileData = user;
          this.editName = user.name ?? '';
          this.editLocation = user.location ?? '';
          this.cdr.detectChanges();
        },
        error: () => { this.profileData = null; }
      });
    }
  }

  startEdit(): void {
    this.editMode = true;
  }

  cancelEdit(): void {
    this.editMode = false;
    if (this.profileData) {
      this.editName = this.profileData.name ?? '';
      this.editLocation = this.profileData.location ?? '';
    }
  }

  saveProfile(): void {
    if (!this.profileData) return;
    this.saving = true;
    const updated = { ...this.profileData, name: this.editName, location: this.editLocation };
    this.userService.update(this.profileData.id, updated).subscribe({
      next: (user: any) => {
        this.profileData = user;
        this.editMode = false;
        this.saving = false;
      },
      error: () => { this.saving = false; }
    });
  }
}
