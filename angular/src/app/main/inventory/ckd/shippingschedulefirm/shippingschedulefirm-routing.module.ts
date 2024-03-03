import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ShippingScheduleFirmComponent } from './shippingschedulefirm.component';

const routes: Routes = [{
    path: '',
    component: ShippingScheduleFirmComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ShippingScheduleFirmRoutingModule {}

