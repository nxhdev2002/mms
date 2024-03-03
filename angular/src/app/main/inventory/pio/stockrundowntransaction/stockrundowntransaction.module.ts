import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StockRundownTransactionRoutingModule } from './stockrundowntransaction-routing.module';
import { StockRundownTransactionComponent } from './stockrundowntransaction.component';

@NgModule({
    declarations: [
        StockRundownTransactionComponent

    ],
    imports: [
        AppSharedModule, StockRundownTransactionRoutingModule]
})
export class StockRundownTransactionModule { }
