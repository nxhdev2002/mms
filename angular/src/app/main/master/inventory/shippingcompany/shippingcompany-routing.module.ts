import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ShippingCompanyComponent } from './shippingcompany.component';

const routes: Routes = [{
    path: '',
    component: ShippingCompanyComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ShippingCompanyRoutingModule {}
