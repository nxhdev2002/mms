import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ModuleRegistry, ClientSideRowModelModule, RowGroupingModule, MenuModule, SetFilterModule, ServerSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule } from '@ag-grid-enterprise/all-modules';
import { StockComponent } from './stock.component';
import { StockRoutingModule } from './stock-routing.module';


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
        StockComponent
    ],
    imports: [
        AppSharedModule,
        StockRoutingModule
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class StockModule { }
