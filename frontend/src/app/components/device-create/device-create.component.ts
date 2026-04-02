import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DeviceService } from '../../services/device.service';

@Component({
  selector: 'app-device-create',
  standalone: false,
  templateUrl: './device-create.component.html',
  styleUrl: './device-create.component.css',
})
export class DeviceCreateComponent implements OnInit {
  deviceForm!: FormGroup;
  submitted = false;
  loading = false;

  constructor(
    private formBuilder: FormBuilder,
    private deviceService: DeviceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.deviceForm = this.formBuilder.group({
      name: ['', Validators.required],
      manufacturer: ['', Validators.required],
      type: ['', Validators.required],
      operatingSystem: ['', Validators.required],
      osVersion: ['', Validators.required],
      processor: ['', Validators.required],
      ram: ['', Validators.required],
      description: [''],
      userId: [null]
    });
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.deviceForm.invalid) {
      return;
    }
    this.loading = true;
    const formData = this.deviceForm.value;

    const cleanedData = {
      ...formData,
      userId: formData.userId ? parseInt(formData.userId) : null
    };

    this.deviceService.create(cleanedData)
      .subscribe({
        next: (res) => {
          console.log('Device created:', res);
          alert('Device created successfully!');
          this.router.navigate(['/devices']);
        },
        error: (err) => {
          console.error('Error creating device:', err);
          alert(err.error?.message || 'An error occurred while creating the device.');
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
        }
      });
  }

  onCancel(): void {
    this.router.navigate(['/devices']);
  }
}


