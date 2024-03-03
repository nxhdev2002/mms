import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ColorRoutingModule } from './color-routing.module';
import { ColorComponent } from './color.component';
import { CreateOrEditColorModalComponent } from './create-or-edit-color-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { AgGridModule } from '@ag-grid-community/angular';
import { ViewHistoryMstCmmColorModalComponent } from './history-color-modal.component';
import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';

// const tabcode_component_dict = {
//     [TABS.APP_MASTER_COMMON_COLOR]: ColorComponent
// };
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
       ColorComponent,
        CreateOrEditColorModalComponent,
        ViewHistoryMstCmmColorModalComponent,

    ],
    imports: [
        AppSharedModule, ColorRoutingModule, AgGridModule]
})
export class ColorModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    // }
}
