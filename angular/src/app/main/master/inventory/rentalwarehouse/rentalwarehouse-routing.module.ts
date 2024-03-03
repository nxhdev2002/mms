import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { RentalWarehouseComponent } from './rentalwarehouse.component';

const routes: Routes = [{
    path: '',
    component: RentalWarehouseComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RentalWarehouseRoutingModule {}
