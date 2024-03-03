import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { MaterialMasterRoutingModule } from './materialmaster-routing.module';
import { MaterialMasterComponent } from './materialmaster.component';
import { viewMaterialMasterModalComponent } from './view-materialmaster-modal.component';
import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';
import { ValidateMaterialMasterModalComponent } from './validate-materialmaster-modal.component';
import { ViewHistoryMaterialMasterModalComponent } from './history-materialmaster-modal.component';


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
       MaterialMasterComponent,
       viewMaterialMasterModalComponent,
       ValidateMaterialMasterModalComponent,
       ViewHistoryMaterialMasterModalComponent

    ],
    imports: [
        AppSharedModule, MaterialMasterRoutingModule]
})
export class MaterialMasterModule {}
