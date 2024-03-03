import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { APlanShiftBaseRoutingModule } from './aplanshiftbase-routing.module';
import { APlanShiftBaseComponent } from './aplanshiftbase.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.ASSY_ADO_APLANSHIFTBASE]: APlanShiftBaseComponent
};

@NgModule({
    declarations: [
       APlanShiftBaseComponent
    ],
    imports: [
        AppSharedModule, APlanShiftBaseRoutingModule
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class APlanShiftBaseModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
