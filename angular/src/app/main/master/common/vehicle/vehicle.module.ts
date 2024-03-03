import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { VehicleComponent } from './vehicle.component';
import { VehicleRoutingModule } from './vehicle-routing.module';
import { CreateOrEditVehicleModalComponent } from './create-or-edit-vehicle-modal.component';
import { ViewVehicleDetailModalComponent } from './view-vehicle-detail-modal.component';
import { ViewMaterialByIdModalComponent } from './view-material-modal.component';
import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';
import { ValidateVehicleModalComponent } from './validate-vehicle-modal.component';
import { ViewHistoryGradeColorModalComponent } from './history-gradecolor-modal.component';

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
       VehicleComponent,
       CreateOrEditVehicleModalComponent,
       ViewVehicleDetailModalComponent,
       ViewMaterialByIdModalComponent,
       ValidateVehicleModalComponent,
       ViewHistoryGradeColorModalComponent
    ],
    imports: [
        AppSharedModule,
        VehicleRoutingModule,

    ]
})
export class VehicleModule {
}
