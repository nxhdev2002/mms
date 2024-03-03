import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ModuleCallingRoutingModule } from './modulecalling-routing.module';
import { ModuleCallingComponent } from './modulecalling.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_MWH_MODULECALLING]: ModuleCallingComponent
};

@NgModule({
    declarations: [
        ModuleCallingComponent
    ],
    imports: [
        AppSharedModule, ModuleCallingRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ModuleCallingModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
