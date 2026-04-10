import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DeviceService } from '../../services/device.service';
import { UserService } from '../../services/user.service';
import { Device } from '../../models/device';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';

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
  myDevicesOnly = false;
  pageTitle = 'All Devices';

  constructor(
    private route: ActivatedRoute,
    private deviceService: DeviceService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private fb: FormBuilder,
  ) {
    this.editForm = this.createEditForm();
  }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.myDevicesOnly = !!data['myDevicesOnly'];
      this.pageTitle = this.myDevicesOnly ? 'My Devices' : 'All Devices';
      this.getDevices();
      this.cdr.detectChanges();
    });
  }

  getDevices(): void {
    const request = this.myDevicesOnly
      ? this.deviceService.getMine()
      : this.deviceService.getAll();

    request.subscribe({
      next: (data) => {
        this.devices = data;
        this.deviceUsers.clear();
        this.cdr.markForCheck();

        this.fetchUsersForDevices();
      },
      error: (err) => console.error(err),
    });
  }

  fetchUsersForDevices(): void {
    this.devices.forEach((device) => {
      if (device.userId) {
        this.userService.getUser(device.userId).subscribe({
          next: (user: any) => {
            this.deviceUsers.set(device.userId!, user);
            this.cdr.markForCheck();
            this.cdr.detectChanges();
          },
          error: (err) => console.error(`Error fetching user ${device.userId}:`, err),
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
        error: (err) => console.error('Error deleting device:', err),
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
      userId: [null],
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
        ...this.editForm.value,
      };

      this.deviceService.update(this.editingDeviceId, updatedDevice).subscribe({
        next: () => {
          console.log('Device updated successfully');
          this.cancelEdit();
          this.getDevices();
        },
        error: (err) => console.error('Error updating device:', err),
      });
    }
  }

  goToDetails(deviceId: number): void {
    this.router.navigate(['/devices', deviceId]);
  }

  assignDevice(deviceId: number): void {
    if (this.myDevicesOnly) {
      return;
    }

    this.deviceService
      .assignDevice(deviceId, this.authService.getUserIdFromToken() || -1)
      .subscribe({
        next: () => {
          console.log('Device assigned successfully');
          this.getDevices();
        },
        error: (err) => console.error('Error assigning device:', err),
      });
  }

  unassignDevice(deviceId: number): void {
    if(this.getUserForDevice(this.devices.find(d => d.id === deviceId)!)?.id !== this.authService.getUserIdFromToken()) {
      alert('You can only unassign devices assigned to you.');
      return;
    }else{
      this.deviceService.unassignDevice(deviceId).subscribe({
        next: () => {
          this.getDevices();
          console.log('Device unassigned successfully'); 
        },
        error: (err) => console.error('Error unassigning device:', err),
      });
    }
  }
}
