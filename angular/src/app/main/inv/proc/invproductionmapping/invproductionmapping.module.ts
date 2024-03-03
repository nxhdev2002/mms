import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InvProductionMappingRoutingModule } from './invproductionmapping-routing.module';
import { InvProductionMappingComponent } from './invproductionmapping.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { ViewHistoryProductionMappingModalComponent } from './history-productionmapping-modal.component';
import { ClientSideRowModelModule, ColumnsToolPanelModule, FiltersToolPanelModule, MenuModule, ModuleRegistry, RowGroupingModule, ServerSideRowModelModule, SetFilterModule } from '@ag-grid-enterprise/all-modules';

const tabcode_component_dict = {
    [TABS.INV_PROC_PRODUCTIONMAPPING]: InvProductionMappingComponent
};
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
        InvProductionMappingComponent,
        ViewHistoryProductionMappingModalComponent
    ],
    imports: [
        AppSharedModule, InvProductionMappingRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class InvProductionMappingModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
