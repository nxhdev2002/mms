import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { RobbingLaneRoutingModule } from './robbinglane-routing.module';
import { RobbingLaneComponent } from './robbinglane.component';
import { CreateOrEditRobbingLaneModalComponent } from './create-or-edit-robbinglane-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_ROBBINGLANE]: RobbingLaneComponent
};

@NgModule({
    declarations: [
       RobbingLaneComponent,
        CreateOrEditRobbingLaneModalComponent

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
