import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StockTransactionRoutingModule } from './stocktransaction-routing.module';
import { StockTransactionComponent } from './stocktransaction.component';

@NgModule({
    declarations: [
        StockTransactionComponent

    ],
    imports: [
        AppSharedModule, StockTransactionRoutingModule]
})
export class StockTransactionModule { }
