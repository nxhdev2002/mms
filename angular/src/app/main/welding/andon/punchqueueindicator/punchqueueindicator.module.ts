import { NgModule } from '@angular/core';
import {  PunchQueueIndicatorRoutingModule } from './punchqueueindicator-routing.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { PunchQueueIndicatorComponent } from './punchqueueindicator.component';

@NgModule({
  imports: [
    PunchQueueIndicatorRoutingModule,
    AppSharedModule,
  ],
  declarations: [
    PunchQueueIndicatorComponent
    ]
})
export class PunchQueueIndicatorModule { }
