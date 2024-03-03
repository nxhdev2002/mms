import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { TotalDelayRoutingModule } from './totaldelay-routing.module';
import { TotalDelayComponent } from './totaldelay.component';
import { CreateOrEditTotalDelayModalComponent } from './create-or-edit-totaldelay-modal.component';

const tabcode_component_dict = {
    [TABS.PTA_ADO_TOTALDELAY]: TotalDelayComponent
}
@NgModule({
    declarations: [
       TotalDelayComponent,
        CreateOrEditTotalDelayModalComponent
    ],
    imports: [
        AppSharedModule, TotalDelayRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class TotalDelayModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
