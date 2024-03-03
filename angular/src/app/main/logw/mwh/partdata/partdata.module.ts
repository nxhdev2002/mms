import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PartDataRoutingModule } from './partdata-routing.module';
import { PartDataComponent } from './partdata.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_MWH_PARTDATA]: PartDataComponent
};
@NgModule({
    declarations: [
       PartDataComponent,

    ],
    imports: [
        AppSharedModule, PartDataRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PartDataModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
