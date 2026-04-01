import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeviceDetailsComponent } from './components/device-details/device-details.component';
import { DeviceCreateComponent } from './components/device-create/device-create.component';
import { DeviceListComponent } from './components/device-list/device-list.component';

const routes: Routes = [
  { path: 'devices/create', component: DeviceCreateComponent },
  { path: 'devices/:id', component: DeviceDetailsComponent },
  { path: 'devices', component: DeviceListComponent },
  { path: '', redirectTo: 'devices', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {onSameUrlNavigation: 'reload'})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
