import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StockReceivingComponent } from './stockreceving.component';
import { StockReceivingRoutingModule } from './stockreceving-routing.module';

@NgModule({
    declarations: [
       StockReceivingComponent, 
    ],
    imports: [
        AppSharedModule, StockReceivingRoutingModule]
})
export class StockReceivingModule {}
