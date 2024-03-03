import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsCalendarRoutingModule } from './gpscalendar-routing.module';
import { GpsCalendarComponent } from './gpscalendar.component';
import { CreateOrEditGpsCalendarModalComponent } from './create-or-edit-gpscalendar-modal.component';

@NgModule({
    declarations: [
       GpsCalendarComponent, 
        CreateOrEditGpsCalendarModalComponent
      
    ],
    imports: [
        AppSharedModule, GpsCalendarRoutingModule]
})
export class GpsCalendarModule {}
