import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PioImpSupplierComponent } from './mstpioimpsupplier.component';

const routes: Routes = [{
    path: '',
    component: PioImpSupplierComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PioImpSupplierRoutingModule {}
