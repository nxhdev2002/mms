import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { VehicleColorTypeComponent } from './vehiclecolortype.component';

const routes: Routes = [{
    path: '',
    component: VehicleColorTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class VehicleColorTypeRoutingModule {}