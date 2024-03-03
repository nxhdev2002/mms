import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ShippingScheduleComponent } from './shippingschedule.component';


const routes: Routes = [{
    path: '',
    component: ShippingScheduleComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ShippingScheduleRoutingModule {}
