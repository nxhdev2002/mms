import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { InvoiceDetailsComponent } from './invoicedetails.component';
import { InvoiceDetailsRoutingModule } from './invoicedetails-routing.module';
import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';


ModuleRegistry.registerModules([
    ClientSideRowModelModule,
    RowGroupingModule,
    MenuModule,
    SetFilterModule,
    ServerSideRowModelModule,
    ColumnsToolPanelModule,
    FiltersToolPanelModule,
]);
@NgModule({
    declarations: [
        InvoiceDetailsComponent,
    ],
    imports: [
        AppSharedModule,
        InvoiceDetailsRoutingModule
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class InvoiceDetailsModule { }
