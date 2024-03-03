import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { TransmissionTypeRoutingModule } from './transmissiontype-routing.module';
import { TransmissionTypeComponent } from './transmissiontype.component';
import { CreateOrEditTransmissionTypeModalComponent } from './create-or-edit-transmissiontype-modal.component';

@NgModule({
    declarations: [
       TransmissionTypeComponent, 
        CreateOrEditTransmissionTypeModalComponent
      
    ],
    imports: [
        AppSharedModule, TransmissionTypeRoutingModule]
})
export class TransmissionTypeModule {}
