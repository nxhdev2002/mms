import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { DailyWorkingTimeRoutingModule } from './dailyworkingtime-routing.module';
import { DailyWorkingTimeComponent } from './dailyworkingtime.component';
import { CreateOrEditDailyWorkingTimeModalComponent } from './create-or-edit-dailyworkingtime-modal.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { FormsModule } from '@angular/forms';
import { MultiSelectModule } from 'primeng/multiselect';
import { ListboxModule } from 'primeng/listbox';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_DAILYWORKINGTIME]: DailyWorkingTimeComponent
}

@NgModule({
    declarations: [
       DailyWorkingTimeComponent,
        CreateOrEditDailyWorkingTimeModalComponent

    ],
    imports: [
        ListboxModule,
        AppSharedModule,
        DailyWorkingTimeRoutingModule,
        BsDropdownModule.forRoot(),
        MultiSelectModule,
        FormsModule,],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class DailyWorkingTimeModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
