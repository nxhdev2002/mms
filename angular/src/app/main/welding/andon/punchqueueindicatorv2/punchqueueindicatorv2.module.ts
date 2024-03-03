import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { Punchqueueindicatorv2RoutingModule } from './punchqueueindicatorv2-routing.module';
import { Punchqueueindicatorv2Component } from './punchqueueindicatorv2.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.ASSY_ADO_ASSEMBLYDATA]: Punchqueueindicatorv2Component
};

@NgModule({
    declarations: [
        Punchqueueindicatorv2Component
    ],
    imports: [
        AppSharedModule, Punchqueueindicatorv2RoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class Punchqueueindicatorv2Module {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
