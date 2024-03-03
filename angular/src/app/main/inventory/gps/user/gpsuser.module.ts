import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsUserRoutingModule } from './gpsuser-routing.module';
import { GpsUserComponent } from './gpsuser.component';

@NgModule({
    declarations: [
       GpsUserComponent,
    ],
    imports: [
        AppSharedModule, GpsUserRoutingModule]
})
export class GpsUserModule {}
