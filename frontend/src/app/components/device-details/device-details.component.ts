import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { UserService } from '../../services/user.service';
import { Device } from '../../models/device';
import { User } from '../../models/user';

@Component({
  selector: 'app-device-details',
  standalone: false,
  templateUrl: './device-details.component.html',
  styleUrl: './device-details.component.css',
})
export class DeviceDetailsComponent implements OnInit {
  device: Device | null = null;
  user: User | null = null;
  loading: boolean = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService,
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.fetchDevice(id);
      }
    });
    this.cdr.detectChanges();
  }

  fetchDevice(id: number): void {
    this.loading = true;
    this.error = null;
    this.deviceService.getDevice(id).subscribe({
      next: (device: any) => {
        this.device = device;
        console.log('Device fetched:', device);
        this.cdr.detectChanges();
        if (device.userId) {
          this.fetchUser(device.userId);
          console.log(`Fetching user with ID ${device.userId} for device ${device.id}`);
        } else {
          this.loading = false;
          this.cdr.detectChanges();
        }
      },
      error: (err) => {
        console.error('Error fetching device:', err);
        this.error = 'Device not found';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  fetchUser(userId: number): void {
    this.userService.getUser(userId).subscribe({
      next: (user:any) => {
        this.user = user;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching user:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/devices']);
  }
}
