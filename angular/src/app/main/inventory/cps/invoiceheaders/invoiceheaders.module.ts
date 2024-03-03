import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InvoiceHeadersRoutingModule } from './invoiceheaders-routing.module';
import { InvoiceHeadersComponent } from './invoiceheaders.component';

@NgModule({
    declarations: [
        InvoiceHeadersComponent,
    ],
    imports: [
        AppSharedModule, InvoiceHeadersRoutingModule]
})
export class InvoiceHeadersModule {}
