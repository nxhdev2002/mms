import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PartManagementComponent } from './partmanagement.component';
import { PartManagementRoutingModule } from './partmanagement-routing.module';

import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';
import { AgGridModule } from '@ag-grid-community/angular';
import { ViewHistoryPartManagementModalComponent } from './history-partmanagement-modal.component';

// Register the required feature modules with the Grid
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
        PartManagementComponent,
        ViewHistoryPartManagementModalComponent,

    ],
    imports: [
        AppSharedModule, PartManagementRoutingModule,AgGridModule]
})
export class PartManagementModule {}
