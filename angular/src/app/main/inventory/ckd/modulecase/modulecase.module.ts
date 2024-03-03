import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ModuleCaseRoutingModule } from './modulecase-routing.module';
import { ModuleCaseComponent } from './modulecase.component';
import { ViewModuleCaseModalComponent } from './view-detail-modulecase-modal.component';
import { FieldsetModule } from 'primeng/fieldset';

import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';
import { AgGridModule } from '@ag-grid-community/angular';

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
        ModuleCaseComponent,
        ViewModuleCaseModalComponent

    ],
    imports: [
        AppSharedModule, ModuleCaseRoutingModule,FieldsetModule,AgGridModule
    ]
})
export class ModuleCaseModule {}
