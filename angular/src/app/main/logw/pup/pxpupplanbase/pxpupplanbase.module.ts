import { CreateOrEditPxPUpPlanBaseModalComponent } from './create-or-edit-pxpupplanbase-modal.component';
import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PxPUpPlanBaseRoutingModule } from './pxpupplanbase-routing.module';
import { PxPUpPlanBaseComponent } from './pxpupplanbase.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_PUP_PXPUPPLANBASE]: PxPUpPlanBaseComponent
};
@NgModule({
    declarations: [
        PxPUpPlanBaseComponent,
        CreateOrEditPxPUpPlanBaseModalComponent
    ],
    imports: [
        AppSharedModule, PxPUpPlanBaseRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PxPUpPlanBaseModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
