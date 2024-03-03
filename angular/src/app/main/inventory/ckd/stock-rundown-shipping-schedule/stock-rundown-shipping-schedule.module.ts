import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StockRundownShippingScheduleComponent } from './stock-rundown-shipping-schedule.component';
import { StockRundownShippingScheduleRoutingModule } from './stock-rundown-shipping-schedule-routing.module';



@NgModule({
    declarations: [
        StockRundownShippingScheduleComponent,
    ],
    imports: [
        AppSharedModule, StockRundownShippingScheduleRoutingModule]
})
export class StockRundownShippingScheduleModule {}
