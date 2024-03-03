import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { LotplanRoutingModule } from './lotplan-routing.module';
import { LotplanComponent } from './lotplan.component';
import { CreateOrEditLotplanModalComponent } from './create-or-edit-lotplan-modal.component';
import {  ImportLotupModalComponent } from './import-lotup-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
  //  [TABS.MASTER_COMMON_LOOKUP]: LotPlanComponent
};

@NgModule({
    declarations: [
        LotplanComponent,
        CreateOrEditLotplanModalComponent,
        ImportLotupModalComponent
    ],
    imports: [
        AppSharedModule, LotplanRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LotplanModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
