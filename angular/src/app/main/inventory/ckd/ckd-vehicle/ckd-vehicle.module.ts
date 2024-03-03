import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CkdVehicleComponent } from './ckd-vehicle.component';
import { CkdVehicleRoutingModule } from './ckd-vehicle-routing.module';
import { ViewCKDVehicleDetailModalComponent } from './view-ckd-vehicle-modal.component';


import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';
import { AgGridModule } from '@ag-grid-community/angular';
import { ViewInterfaceComponent } from './view-interface.component';
import { EditCKDVehicleDetailModalComponent } from './edit-ckd-vehicle-modal.component';
import { ExportExcelCkdVehicleComponent } from './export-excel-ckd-vehicle-modal.component';


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
        CkdVehicleComponent,
        ViewCKDVehicleDetailModalComponent,
        ViewInterfaceComponent,
        EditCKDVehicleDetailModalComponent,
        ExportExcelCkdVehicleComponent
    ],
    imports: [
        AppSharedModule,CkdVehicleRoutingModule, AgGridModule
    ]
})
export class CkdVehicleModule {
}


