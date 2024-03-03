import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StockReceivingTransactionRoutingModule } from './stockreceivingtransaction-routing.module';
import { StockReceivingTransactionComponent } from './stockreceivingtransaction.component';
import { ImportGpsStockReceivingTransactionComponent } from './import-stockreceivingtransaction.component';
import { ListErrorImportGpsStockReceivingTransactionComponent } from './list-error-import-stockreceivingtransaction-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        StockReceivingTransactionRoutingModule,
    ],
    declarations: [
        StockReceivingTransactionComponent,
        ImportGpsStockReceivingTransactionComponent,
        ListErrorImportGpsStockReceivingTransactionComponent

    ]
})
export class StockReceivingTransactionModule { }
