import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InvoiceRoutingModule } from './invoice-routing.module';
import { InvoiceComponent } from './invoice.component';
import { ViewHistoryInvoiceModalComponent } from './history-invoice-modal.component';

@NgModule({
    declarations: [
        InvoiceComponent,
        ViewHistoryInvoiceModalComponent
    ],
    imports: [
        AppSharedModule,
        InvoiceRoutingModule,
    ]
})
export class InvoiceModule {
}


