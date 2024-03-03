import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { ContDevanningLTRoutingModule } from './contdevanninglt-routing.module';
import { ContDevanningLTComponent } from './contdevanninglt.component';
import { CreateOrEditContDevanningLTModalComponent } from './create-or-edit-contdevanninglt-modal.component';
import { ImportContDevanningLTComponent } from './import-contdevanninglt-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_CONTDEVANNINGLT]: ContDevanningLTComponent
};

@NgModule({
    declarations: [
    ContDevanningLTComponent,
    CreateOrEditContDevanningLTModalComponent,
    ImportContDevanningLTComponent
    ],
    imports: [
        AppSharedModule, ContDevanningLTRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ContDevanningLTModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
