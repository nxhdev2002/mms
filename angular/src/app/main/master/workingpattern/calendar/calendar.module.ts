import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { CalendarRoutingModule } from './calendar-routing.module';
import { CalendarComponent } from './calendar.component';
import { CreateOrEditCalendarModalComponent } from './create-or-edit-calendar-modal.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ViewDetailCalendarModalComponent } from './details-calendar-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_CALENDAR]: CalendarComponent
};

@NgModule({
    declarations: [
        CalendarComponent,
        CreateOrEditCalendarModalComponent,
        ViewDetailCalendarModalComponent
    ],
    imports: [
        AppSharedModule,
        CalendarRoutingModule,
        BsDatepickerModule.forRoot()
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class CalendarModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
 }
