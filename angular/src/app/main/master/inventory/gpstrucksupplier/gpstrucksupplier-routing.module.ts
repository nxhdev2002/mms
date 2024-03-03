import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsTruckSupplierComponent } from './gpstrucksupplier.component';

const routes: Routes = [{
    path: '',
    component: GpsTruckSupplierComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsTruckSupplierRoutingModule {}
