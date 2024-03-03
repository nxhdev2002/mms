import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { LineEfficiencyDetailsRoutingModule } from './lineefficiencydetails-routing.module';
import { LineEfficiencyDetailsComponent } from './lineefficiencydetails.component';
import { CreateOrEditLineEfficiencyDetailsModalComponent } from './create-or-edit-lineefficiencydetails-modal.component';

const tabcode_component_dict = {
    [TABS.PTA_ADO_LINEEFFICIENCYDETAILS]: LineEfficiencyDetailsComponent
}


@NgModule({
    declarations: [
       LineEfficiencyDetailsComponent,
        CreateOrEditLineEfficiencyDetailsModalComponent

    ],
    imports: [
        AppSharedModule, LineEfficiencyDetailsRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LineEfficiencyDetailsModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
