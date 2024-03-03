import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PickingSignalRoutingModule } from './pickingsignal-routing.module';
import { PickingSignalComponent } from './pickingsignal.component';
//import { CreateOrEditPickingSignalModalComponent } from './create-or-edit-pickingsignal-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_PIK_PICKINGSIGNAL]: PickingSignalComponent
};
@NgModule({
    declarations: [
       PickingSignalComponent  
    ],
    imports: [
        AppSharedModule, PickingSignalRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PickingSignalModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
