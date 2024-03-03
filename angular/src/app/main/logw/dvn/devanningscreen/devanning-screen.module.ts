import {CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DevanningScreenRoutingModule } from './devanning-screen-routing.module';
import { DevanningScreenComponent } from './devanning-screen.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_DVN_DEVANNINGSCREEN]: DevanningScreenComponent
};

@NgModule({
    declarations: [
        DevanningScreenComponent
    ],
    imports: [
        AppSharedModule, DevanningScreenRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class DevanningScreenModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
