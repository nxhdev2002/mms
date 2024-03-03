import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StockRundownShippingResultComponent } from './stock-rundown-shipping-result.component';
import { StockRundownShippingResultRoutingModule } from './stock-rundown-shipping-result-routing.module';


@NgModule({
    declarations: [
        StockRundownShippingResultComponent,
    ],
    imports: [
        AppSharedModule, StockRundownShippingResultRoutingModule]
})
export class StockRundownShippingResultModule {}
