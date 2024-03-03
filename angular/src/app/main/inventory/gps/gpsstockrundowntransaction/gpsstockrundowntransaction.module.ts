
import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsStockRundownTransactionRoutingModule } from './gpsstockrundowntransaction-routing.module';
import { GpsStockRundownTransactionComponent } from './gpsstockrundowntransaction.component';

@NgModule({
    declarations: [
       GpsStockRundownTransactionComponent,
    ],
    imports: [
        AppSharedModule, GpsStockRundownTransactionRoutingModule]
})
export class GpsStockRundownTransactionModule {}
