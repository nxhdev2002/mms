import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StockRundownWearehouseComponent } from './stock-rundown-warehouse.component';
import { StockRundownWearehouseRoutingModule } from './stock-rundown-warehouse-routing.module';


@NgModule({
    declarations: [
        StockRundownWearehouseComponent,
    ],
    imports: [
        AppSharedModule,StockRundownWearehouseRoutingModule]
})
export class StockRundownWearehouseModule {}
