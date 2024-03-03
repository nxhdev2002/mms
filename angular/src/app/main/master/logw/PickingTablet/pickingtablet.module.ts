import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { PickingTabletRoutingModule } from './pickingtablet-routing.module';
import { PickingTabletComponent } from './pickingtablet.component';
import { CreateOrEditPickingTabletModalComponent } from './create-or-edit-pickingtablet-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_PICKINGTABLET]: PickingTabletComponent
};


@NgModule({
    declarations: [
       PickingTabletComponent,
        CreateOrEditPickingTabletModalComponent

    ],
    imports: [
        AppSharedModule, PickingTabletRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PickingTabletModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
