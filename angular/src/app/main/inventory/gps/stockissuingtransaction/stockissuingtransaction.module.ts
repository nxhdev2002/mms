import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StockIssuingTransactionComponent } from './stockissuingtransaction.component';
import { StockIssuingTransactionRoutingModule } from './stockissuingtransaction-routing.module';
import { ViewGpsStockIssuingTransactionModalComponent } from './view-stockissuingtrans-modal.component';
import { ImportGpsStockIssuingTransactionComponent } from './import-stockissuingtransaction.component';
import { ListErrorImportGpsStockIssuingTransactionComponent } from './list-error-import-stockissuingtransaction-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        StockIssuingTransactionRoutingModule,
    ],
    declarations: [
        StockIssuingTransactionComponent,
        ViewGpsStockIssuingTransactionModalComponent,
        ImportGpsStockIssuingTransactionComponent,
        ListErrorImportGpsStockIssuingTransactionComponent
    ]
})
export class StockIssuingTransactionModule { }
