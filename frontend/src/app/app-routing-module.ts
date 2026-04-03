import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeviceDetailsComponent } from './components/device-details/device-details.component';
import { DeviceCreateComponent } from './components/device-create/device-create.component';
import { DeviceListComponent } from './components/device-list/device-list.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './auth.guard';

const routes: Routes = [
  { path: 'devices/create', component: DeviceCreateComponent, canActivate: [authGuard] },
  { path: 'devices/:id', component: DeviceDetailsComponent, canActivate: [authGuard] },
  { path: 'devices', component: DeviceListComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {onSameUrlNavigation: 'reload'})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
