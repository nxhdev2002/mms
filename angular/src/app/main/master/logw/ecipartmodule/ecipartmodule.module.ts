import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { EciPartModuleRoutingModule } from './ecipartmodule-routing.module';
import { EciPartModuleComponent } from './ecipartmodule.component';


const tabcode_component_dict = {
    [TABS.MASTER_LOGW_ECIPARTMODULE]: EciPartModuleComponent
};


@NgModule({
    declarations: [
       EciPartModuleComponent,
    ],
    imports: [
        AppSharedModule, EciPartModuleRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class EciPartModuleModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
