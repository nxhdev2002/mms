import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CallinglightRoutingModule } from './callinglight-routing.module';
import { CallinglightComponent } from './callinglight.component';

@NgModule({
    declarations: [
       CallinglightComponent,
    ],
    imports: [
        AppSharedModule, CallinglightRoutingModule]
})
export class CallinglightModule {}
