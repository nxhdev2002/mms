import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CallinglightstatusRoutingModule } from './callinglightstatus-routing.module';
import { CallinglightstatusComponent } from './callinglightstatus.component';
import { CreateOrEditCallinglightstatusModalComponent } from './create-or-edit-callinglightstatus-modal.component';

@NgModule({
    declarations: [
        CallinglightstatusComponent,
        CreateOrEditCallinglightstatusModalComponent

    ],
    imports: [
        AppSharedModule, CallinglightstatusRoutingModule]
})
export class CallinglightstatusModule {
}
