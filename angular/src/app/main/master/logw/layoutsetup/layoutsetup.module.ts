import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { LayoutSetupRoutingModule } from './layoutsetup-routing.module';
import { LayoutSetupComponent } from './layoutsetup.component';
import { CreateOrEditLayoutSetupModalComponent } from './create-or-edit-layoutsetup-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_LAYOUTSETUP]: LayoutSetupComponent
};

@NgModule({
    declarations: [
       LayoutSetupComponent,
        CreateOrEditLayoutSetupModalComponent
    ],
    imports: [
        AppSharedModule, LayoutSetupRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LayoutSetupModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
