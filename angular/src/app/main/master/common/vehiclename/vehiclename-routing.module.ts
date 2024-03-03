import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { VehicleNameComponent } from './vehiclename.component';

const routes: Routes = [{
    path: '',
    component: VehicleNameComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class VehicleNameRoutingModule {}