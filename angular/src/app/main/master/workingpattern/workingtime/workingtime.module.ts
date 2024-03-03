import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { WorkingTimeRoutingModule } from './workingtime-routing.module';
import { WorkingTimeComponent } from './workingtime.component';
import { CreateOrEditWorkingTimeModalComponent } from './create-or-edit-workingtime-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_WORKINGTIME]: WorkingTimeComponent
}

@NgModule({
    declarations: [
        WorkingTimeComponent,
        CreateOrEditWorkingTimeModalComponent
    ],
    imports: [
        AppSharedModule, WorkingTimeRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class WorkingTimeModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
