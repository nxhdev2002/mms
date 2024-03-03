import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsUserComponent } from './gpsuser.component';

const routes: Routes = [{
    path: '',
    component: GpsUserComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsUserRoutingModule {}
