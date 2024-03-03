import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { AinplanRoutingModule } from './ainplan-routing.module';
import { AinplanComponent } from './ainplan.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { ViewAInPlanDetailsModalComponent } from './view-ainplan-detail-modal.component';
import { ViewHistoryAInPlanComponent } from './history-ainplan-modal.component';

const tabcode_component_dict = {
    [TABS.ASSY_ADO_AINPLAN]: AinplanComponent
};

@NgModule({
    declarations: [
        AinplanComponent,
        ViewAInPlanDetailsModalComponent,
        ViewHistoryAInPlanComponent
    ],
    imports: [
        AppSharedModule,
        AinplanRoutingModule
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class AinplanModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
