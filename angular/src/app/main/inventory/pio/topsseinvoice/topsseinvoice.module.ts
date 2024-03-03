import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TopsseInvoiceRoutingModule } from './topsseinvoice-routing.module';
import { TopsseInvoiceComponent } from './topsseinvoice.component';
import { ReceiveTopsseInvoiceComponent } from './receive-topsseinvoice-modal.component';

@NgModule({
    declarations: [
        TopsseInvoiceComponent,
        ReceiveTopsseInvoiceComponent
    ],
    imports: [
        AppSharedModule,
        TopsseInvoiceRoutingModule
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TopsseInvoiceModule { }
