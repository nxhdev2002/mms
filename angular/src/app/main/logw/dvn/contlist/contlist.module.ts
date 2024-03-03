import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ContListRoutingModule } from './contlist-routing.module';
import { ContListComponent } from './contlist.component';
import { CreateOrEditContListModalComponent } from './create-or-edit-contlist-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_DVN_CONTLIST]: ContListComponent
};

@NgModule({
    declarations: [
       ContListComponent,
        CreateOrEditContListModalComponent

    ],
    imports: [
        AppSharedModule, ContListRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ContListModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
