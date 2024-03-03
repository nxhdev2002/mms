import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PlcSignalRoutingModule } from './plcsignal-routing.module';
import { PlcSignalComponent } from './plcsignal.component';
import { CreateOrEditPlcSignalModalComponent } from './create-or-edit-plcsignal-modal.component';

@NgModule({
    declarations: [
       PlcSignalComponent,
        CreateOrEditPlcSignalModalComponent

    ],
    imports: [
        AppSharedModule, PlcSignalRoutingModule]
})
export class PlcSignalModule {}
