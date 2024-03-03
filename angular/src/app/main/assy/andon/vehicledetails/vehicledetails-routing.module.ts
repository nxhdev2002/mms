import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { VehicleDetailsComponent } from './vehicledetails.component';

const routes: Routes = [{
    path: '',
    component: VehicleDetailsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class VehicleDetailsRoutingModule {}
