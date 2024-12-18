import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { FuelTypeComponent } from './fueltype.component';

const routes: Routes = [{
    path: '',
    component: FuelTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FuelTypeRoutingModule {}
