import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';

import { WeekRoutingModule } from './week-routing.module';
import { WeekComponent } from './week.component';
import { CreateOrEditWeekModalComponent } from './create-or-edit-week-modal.component';

import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_WEEK]: WeekComponent
}

@NgModule({
    declarations: [
        WeekComponent,
        CreateOrEditWeekModalComponent
    ],
    imports: [
        AppSharedModule, WeekRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class WeekModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
