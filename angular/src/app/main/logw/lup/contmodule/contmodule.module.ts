import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ContModuleRoutingModule } from './contmodule-routing.module';
import { ContModuleComponent } from './contmodule.component';
import { CreateOrEditContModuleModalComponent } from './create-or-edit-contmodule-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_LUP_CONTMODULE]: ContModuleComponent
};

@NgModule({
    declarations: [
       ContModuleComponent,
        CreateOrEditContModuleModalComponent
    ],
    imports: [
        AppSharedModule, ContModuleRoutingModule]
})
export class ContModuleModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
