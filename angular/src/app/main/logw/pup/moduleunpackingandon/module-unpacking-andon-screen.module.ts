import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ModuleUnpackingAndonRoutingModule } from './module-unpacking-andon-screen-routing.module';
import { ModuleUnpackingAndonComponent } from './module-unpacking-andon-screen.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_PUP_MODULEUNPACKINGANDON]: ModuleUnpackingAndonComponent
};

@NgModule({
    declarations: [
        ModuleUnpackingAndonComponent
    ],
    imports: [
        AppSharedModule, ModuleUnpackingAndonRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ModuleUnpackingAndonModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
