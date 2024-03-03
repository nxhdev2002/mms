import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { RobbingLaneRoutingModule } from './robbinglane-routing.module';
import { RobbingLaneComponent } from './robbinglane.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_MWH_ROBBINGLANE]: RobbingLaneComponent
};
@NgModule({
    declarations: [
       RobbingLaneComponent,    
    ],
    imports: [
        AppSharedModule, RobbingLaneRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class RobbingLaneModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}