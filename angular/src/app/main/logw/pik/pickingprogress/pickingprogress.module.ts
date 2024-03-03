import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PickingProgressRoutingModule } from './pickingprogress-routing.module';
import { PickingProgressComponent } from './pickingprogress.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_PIK_PICKINGPROGRESS]: PickingProgressComponent
};
@NgModule({
    declarations: [
       PickingProgressComponent,

    ],
    imports: [
        AppSharedModule, PickingProgressRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PickingProgressModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
