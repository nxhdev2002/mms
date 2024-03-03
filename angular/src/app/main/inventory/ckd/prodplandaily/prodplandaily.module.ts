import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ProdPlanDailyRoutingModule } from './prodplandaily-routing.module';
import { ProdPlanDailyComponent } from './prodplandaily.component';

import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';
import { AgGridModule } from '@ag-grid-community/angular';
import { ViewHistoryProdPlanDailyModalComponent } from './history-prodplandaily-modal.component';

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
       ProdPlanDailyComponent,
       ViewHistoryProdPlanDailyModalComponent,

    ],
    imports: [
        AppSharedModule, ProdPlanDailyRoutingModule, AgGridModule]
})
export class ProdPlanDailyModule {}
