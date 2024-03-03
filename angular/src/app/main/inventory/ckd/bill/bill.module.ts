import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BillRoutingModule } from './bill-routing.module';
import { BillComponent } from './bill.component';
import { ViewHistoryBillModalComponent } from './history-bill-modal.component';

@NgModule({
    declarations: [
       BillComponent, 
       ViewHistoryBillModalComponent,
    ],
    imports: [
        AppSharedModule, BillRoutingModule]
})
export class BillModule {}
