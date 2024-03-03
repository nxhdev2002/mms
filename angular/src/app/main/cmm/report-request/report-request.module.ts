import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {ReportRequestRoutingModule } from './report-request-routing.module';
import {ReportRequestComponent } from './report-request.component';
import {RequestModalComponent } from './request-modal.component';

@NgModule({
    declarations: [
        ReportRequestComponent,
       RequestModalComponent
    ],
    imports: [
        AppSharedModule, ReportRequestRoutingModule]
})
export class ReportRequestModule {}
