import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DeviceService } from '../../services/device.service';
import { UserService } from '../../services/user.service';
import { Device } from '../../models/device';
import { User } from '../../models/user';

@Component({
  selector: 'app-device-list',
  standalone: false,
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.css',
})
export class DeviceListComponent implements OnInit {
  devices: Device[] = [];
  deviceUsers: Map<number, User> = new Map();
  editingDeviceId: number | null = null;
  editForm: FormGroup;
  editingDevice: Device | null = null;

  constructor(
    private deviceService: DeviceService,
    private userService: UserService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private fb: FormBuilder
  ) {
    this.editForm = this.createEditForm();
  }

  ngOnInit(): void {
    console.log('DeviceListComponent initialized');
    this.getDevices();
    this.cdr.detectChanges();
  }


  getDevices(): void {
    this.deviceService.getAll()
      .subscribe({
        next: (data) => {
          console.log('Devices fetched:', data);
          this.devices = data;
          this.cdr.markForCheck();
          
          this.fetchUsersForDevices();
        },
        error: (err) => console.error(err)
      });
  }

  fetchUsersForDevices(): void {
    this.devices.forEach(device => {
      if (device.userId) {
        console.log(`Fetching user for device ${device.id} with userId ${device.userId}`);
        this.userService.getUser(device.userId).subscribe({
          next: (user: any) => {
            this.deviceUsers.set(device.userId!, user);
            this.cdr.markForCheck();
            this.cdr.detectChanges();
          },
          error: (err) => console.error(`Error fetching user ${device.userId}:`, err)
        });
      }
    });
  }

  getUserForDevice(device: Device): User | undefined {
    return device.userId ? this.deviceUsers.get(device.userId) : undefined;
  }

  deleteDevice(id: number): void {
    if (confirm('Are you sure you want to delete this device?')) {
      this.deviceService.delete(id).subscribe({
        next: () => {
          console.log('Device deleted');
          this.getDevices();
        },
        error: (err) => console.error('Error deleting device:', err)
      });
    }
  }

  createEditForm(): FormGroup {
    return this.fb.group({
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

  startEdit(device: Device): void {
    this.editingDeviceId = device.id || null;
    this.editingDevice = { ...device };
    this.editForm.patchValue(device);
    this.cdr.detectChanges();
  }

  cancelEdit(): void {
    this.editingDeviceId = null;
    this.editingDevice = null;
    this.editForm.reset();
    this.cdr.detectChanges();
  }

  saveEdit(): void {
    if (this.editForm.valid && this.editingDeviceId) {
      const updatedDevice: Device = {
        ...this.editingDevice,
        ...this.editForm.value
      };
      
      this.deviceService.update(this.editingDeviceId, updatedDevice).subscribe({
        next: () => {
          console.log('Device updated successfully');
          this.cancelEdit();
          this.getDevices();
        },
        error: (err) => console.error('Error updating device:', err)
      });
    }
  }

  updateDevice(id: number): void {}
  
}