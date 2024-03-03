import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { CallingLightRoutingModule } from './callinglight-routing.module';
import { CallingLightComponent } from './callinglight.component';
import { CreateOrEditCallingLightModalComponent } from './create-or-edit-callinglight-modal.component';


const tabcode_component_dict = {
    [TABS.MASTER_LOGW_CALLINGLIGHT]: CallingLightComponent
};


@NgModule({
    declarations: [
       CallingLightComponent,
        CreateOrEditCallingLightModalComponent

    ],
    imports: [
        AppSharedModule, CallingLightRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class CallingLightModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
