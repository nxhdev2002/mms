import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';

//Component
import { LookupRoutingModule } from './lookup-routing.module';
import { LookupComponent } from './lookup.component';
import { CreateOrEditLookupModalComponent } from './create-or-edit-lookup-modal.component';

import { TABS } from '@app/shared/constants/tab-keys';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ViewHistoryMstCmmLookupModalComponent } from './history-lookup-modal.component';
import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';



// const tabcode_component_dict = {
//     [TABS.MASTER_COMMON_LOOKUP]: LookupComponent
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
       LookupComponent,
       CreateOrEditLookupModalComponent,
       ViewHistoryMstCmmLookupModalComponent
    ],
    imports: [
        AppSharedModule,
        LookupRoutingModule
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LookupModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    // }
}
