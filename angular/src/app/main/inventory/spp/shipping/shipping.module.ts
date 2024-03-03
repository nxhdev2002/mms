import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ShippingComponent } from './shipping.component';
import { ShippingRoutingModule } from './shipping-routing.module';
import { ModuleRegistry, ClientSideRowModelModule, RowGroupingModule, MenuModule, SetFilterModule, ServerSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule } from '@ag-grid-enterprise/all-modules';
import { ExcelSummaryModalComponent } from './excel-summary-modal.component';


// ModuleRegistry.registerModules([
//     ClientSideRowModelModule,
//     RowGroupingModule,
//     MenuModule,
//     SetFilterModule,
//     ServerSideRowModelModule,
//     ColumnsToolPanelModule,
//     FiltersToolPanelModule,
// ]);

@NgModule({
    declarations: [
        ShippingComponent,
        ExcelSummaryModalComponent
    ],
    imports: [
        AppSharedModule,
        ShippingRoutingModule
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ShippingModule { }
