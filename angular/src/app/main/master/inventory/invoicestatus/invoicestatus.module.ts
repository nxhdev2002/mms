import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { InvoiceStatusRoutingModule } from './invoicestatus-routing.module';
import { InvoiceStatusComponent } from './invoicestatus.component';

@NgModule({
    declarations: [
        InvoiceStatusComponent


    ],
    imports: [
        AppSharedModule, InvoiceStatusRoutingModule]
})
export class InvoiceStatusModule { }
