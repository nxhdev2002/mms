import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PunchQueueRoutingModule } from './punchqueue-routing.module';
import { PunchQueueComponent } from './punchqueue.component';
import { CreateOrEditPunchQueueModalComponent } from './create-or-edit-punchqueue-modal.component';

@NgModule({
    declarations: [
       PunchQueueComponent,
        CreateOrEditPunchQueueModalComponent

    ],
    imports: [
        AppSharedModule, PunchQueueRoutingModule]
})
export class PunchQueueModule {}
