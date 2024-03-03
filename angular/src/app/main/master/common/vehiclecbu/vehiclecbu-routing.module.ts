import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleCBUComponent } from './vehiclecbu.component';

const routes: Routes = [{
    path: '',
    component: VehicleCBUComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class VehicleCBURoutingModule { }
