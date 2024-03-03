import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { WeldingProgressRoutingModule } from './weldingprogress-routing.module';
import { WeldingProgressComponent } from './weldingprogress.component';
import { CreateOrEditWeldingProgressModalComponent } from './create-or-edit-weldingprogress-modal.component';

@NgModule({
    declarations: [
       WeldingProgressComponent,
        CreateOrEditWeldingProgressModalComponent

    ],
    imports: [
        AppSharedModule, WeldingProgressRoutingModule]
})
export class WeldingProgressModule {}
