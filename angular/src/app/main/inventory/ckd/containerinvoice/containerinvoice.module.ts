import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ContainerInvoiceRoutingModule } from './containerinvoice-routing.module';
import { ContainerInvoiceComponent } from './containerinvoice.component';
import { CreateOrEditContainerInvoiceModalComponent } from './create-or-edit-containerinvoice-modal.component';
import { ViewCustomsModalComponent } from './view-customs-modal.component';
import { ViewHistoryContainerInvoiceModalComponent } from './history-container-invoice-modal.component';

@NgModule({
    declarations: [
       ContainerInvoiceComponent,
        CreateOrEditContainerInvoiceModalComponent,
        ViewCustomsModalComponent,
        ViewHistoryContainerInvoiceModalComponent
    ],
    imports: [
        AppSharedModule, ContainerInvoiceRoutingModule],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ContainerInvoiceModule {}
