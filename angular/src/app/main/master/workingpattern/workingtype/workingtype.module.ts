import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { WorkingTypeRoutingModule } from './workingtype-routing.module';
import { WorkingTypeComponent } from './workingtype.component';
import { CreateOrEditWorkingTypeModalComponent } from './create-or-edit-workingtype-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_WORKINGTYPE]: WorkingTypeComponent
}

@NgModule({
    declarations: [
        WorkingTypeComponent,
        CreateOrEditWorkingTypeModalComponent
    ],
    imports: [
        AppSharedModule, WorkingTypeRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})

export class WorkingTypeModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
 }
