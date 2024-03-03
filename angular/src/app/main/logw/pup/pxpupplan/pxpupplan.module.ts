import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CreateOrEditPxPUpPlanModalComponent } from './create-or-edit-pxpupplan-modal.component';
import { ImportPxpUpPlanComponent } from './import-pxpupplan-modal.component';
import { PxPUpPlanRoutingModule } from './pxpupplan-routing.module';
import { PxPUpPlanComponent } from './pxpupplan.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_PUP_PXPUPPLAN]: PxPUpPlanComponent
};

@NgModule({
    declarations: [
       PxPUpPlanComponent,
        CreateOrEditPxPUpPlanModalComponent,
        ImportPxpUpPlanComponent
    ],
    imports: [
        AppSharedModule, PxPUpPlanRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PxPUpPlanModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
