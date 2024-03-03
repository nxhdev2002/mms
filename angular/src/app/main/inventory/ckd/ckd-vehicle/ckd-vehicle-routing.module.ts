import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

import { CkdVehicleComponent } from './ckd-vehicle.component';

const routes: Routes = [{
    path: '',
    component: CkdVehicleComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CkdVehicleRoutingModule {}
