import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsScreenSettingRoutingModule } from './gpsscreensetting-routing.module';
import { GpsScreenSettingComponent } from './gpsscreensetting.component';
import { CreateOrEditGpsScreenSettingModalComponent } from './create-or-edit-gpsscreensetting-modal.component';

@NgModule({
    declarations: [
       GpsScreenSettingComponent,
        CreateOrEditGpsScreenSettingModalComponent

    ],
    imports: [
        AppSharedModule, GpsScreenSettingRoutingModule]
})
export class GpsScreenSettingModule {}
