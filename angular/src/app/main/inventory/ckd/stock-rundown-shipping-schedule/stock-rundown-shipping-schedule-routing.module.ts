import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StockRundownShippingScheduleComponent } from './stock-rundown-shipping-schedule.component';

const routes: Routes = [{
    path: '',
    component: StockRundownShippingScheduleComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockRundownShippingScheduleRoutingModule {}
