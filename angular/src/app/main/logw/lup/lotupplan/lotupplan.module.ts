import { CUSTOM_ELEMENTS_SCHEMA,NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { LotUpPlanRoutingModule } from './lotupplan-routing.module';
import { LotUpPlanComponent } from './lotupplan.component';
import { CreateOrEditLotUpPlanModalComponent } from './create-or-edit-lotupplan-modal.component';
import { ImportLotupplanModalComponent } from './import-lotupplan-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_LUP_LOTUPPLAN]: LotUpPlanComponent
};

@NgModule({
    declarations: [
        LotUpPlanComponent,
        CreateOrEditLotUpPlanModalComponent,
        ImportLotupplanModalComponent
    ],
    imports: [
        AppSharedModule, LotUpPlanRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LotUpPlanModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
