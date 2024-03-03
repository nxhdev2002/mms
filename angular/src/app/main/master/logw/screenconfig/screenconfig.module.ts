import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { ScreenConfigRoutingModule } from './screenconfig-routing.module';
import { ScreenConfigComponent } from './screenconfig.component';
import { CreateOrEditScreenConfigModalComponent } from './create-or-edit-screenconfig-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_SCREENCONFIG]: ScreenConfigComponent
};

@NgModule({
    declarations: [
       ScreenConfigComponent,
        CreateOrEditScreenConfigModalComponent

    ],
    imports: [
        AppSharedModule, ScreenConfigRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ScreenConfigModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
