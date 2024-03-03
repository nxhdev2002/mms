import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { PickingTabletSetupRoutingModule } from './pickingtabletsetup-routing.module';
import { PickingTabletSetupComponent } from './pickingtabletsetup.component';
import { CreateOrEditPickingTabletSetupModalComponent } from './create-or-edit-pickingtabletsetup-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_PICKINGTABLETSETUP]: PickingTabletSetupComponent
};

@NgModule({
    declarations: [
        PickingTabletSetupComponent,
        CreateOrEditPickingTabletSetupModalComponent
    ],
    imports: [
        AppSharedModule, PickingTabletSetupRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PickingTabletSetupModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
