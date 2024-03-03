import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { WeldingSignalRoutingModule } from './weldingsignal-routing.module';
import { WeldingSignalComponent } from './weldingsignal.component';
import { CreateOrEditWeldingSignalModalComponent } from './create-or-edit-weldingsignal-modal.component';

@NgModule({
    declarations: [
       WeldingSignalComponent,
        CreateOrEditWeldingSignalModalComponent

    ],
    imports: [
        AppSharedModule, WeldingSignalRoutingModule]
})
export class WeldingSignalModule {}
