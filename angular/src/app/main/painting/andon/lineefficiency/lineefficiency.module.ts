import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { LineEfficiencyRoutingModule } from './lineefficiency-routing.module';
import { LineEfficiencyComponent } from './lineefficiency.component';
import { CreateOrEditLineEfficiencyModalComponent } from './create-or-edit-lineefficiency-modal.component';

const tabcode_component_dict = {
    [TABS.PTA_ADO_LINEEFFICIENCY]: LineEfficiencyComponent
}


@NgModule({
    declarations: [
       LineEfficiencyComponent,
        CreateOrEditLineEfficiencyModalComponent
    ],
    imports: [
        AppSharedModule, LineEfficiencyRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LineEfficiencyModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
