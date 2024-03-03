import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ContainerListRoutingModule } from './containerlist-routing.module';
import { ContainerListComponent } from './containerlist.component';
import { ViewContainerListModalComponent } from './view-detail-containerlist-modal.component';
import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';
import { AgGridModule } from '@ag-grid-community/angular';
import { ViewDetailCalendarModalComponent } from '@app/main/master/workingpattern/calendar/details-calendar-modal.component';
import { ViewContainerIntransitComponent } from './view-containerintransit-modal.component';

import { ViewImportDevanComponent } from './view-importdevan.component';
import { ViewHistoryContainerListModalComponent } from './history-containerlist-modal.component';
import { ExportExcelContainerListComponent } from './export-excel-containerlist-modal.component';



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
       ContainerListComponent,
       ViewContainerListModalComponent,
       ViewContainerIntransitComponent,
       ViewImportDevanComponent,
       ViewHistoryContainerListModalComponent,
       ExportExcelContainerListComponent

    ],
    imports: [
        AppSharedModule, ContainerListRoutingModule, AgGridModule
      ],
      schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ContainerListModule {}
