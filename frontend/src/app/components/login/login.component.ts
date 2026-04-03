import { Component ,ChangeDetectorRef, inject} from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { LoginRequest } from '../../models/loginRequest';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;
  registerForm: FormGroup;
  submitted = false;
  isLogin = true;
  private router = inject(Router);

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });

    this.registerForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        name: ['', Validators.required],
        location: ['', Validators.required],
        role: 'user',
        password: ['', Validators.required],
        confirmPassword: ['', Validators.required],
      },
      { validators: this.passwordMatchValidator },
    );
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.registerForm.valid && !this.isLogin) {
      const registerRequest = {
        ...this.registerForm.value,
        role: 'User',
      };
      this.authService.register(registerRequest).subscribe({
        next: (response: any) => {
          console.log('Registration successful:', response);
          alert('Registration successful! Please log in.');
          this.toggleForm(true);
          this.cdr.detectChanges();
          
        },
        error: (error) => {
          console.error('Registration failed:', error);
          alert(error.error?.message || 'Registration failed. Please try again.');
        },
      });
    } else if (this.loginForm.valid && this.isLogin) {
      if (this.loginForm.invalid) {
        return;
      }

      const loginRequest: LoginRequest = this.loginForm.value;
      this.authService.login(loginRequest).subscribe({
        next: (response: any) => {
          console.log('Login successful:', response);
          localStorage.setItem('token', response.token);
          console.log('Token stored in localStorage:', localStorage.getItem('token'));
          this.router.navigate(['/devices']);
          
          alert('Login successful!');
        },
        error: (error) => {

          alert(error.error?.message || 'Login failed. Please check your credentials and try again.');
        },
      });
    }
  }

  toggleForm(isLogin: boolean): void {
    this.isLogin = isLogin;
    this.submitted = false;
  }

  passwordMatchValidator(form: AbstractControl) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }
}
