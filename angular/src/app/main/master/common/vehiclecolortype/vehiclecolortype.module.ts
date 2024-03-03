import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { VehicleColorTypeRoutingModule } from './vehiclecolortype-routing.module';
import { VehicleColorTypeComponent } from './vehiclecolortype.component';
import { viewVehicleColorTypeModalComponent } from './view-vehiclecolortype-modal.component';

@NgModule({
    declarations: [
        viewVehicleColorTypeModalComponent,
        VehicleColorTypeComponent


    ],
    imports: [
        AppSharedModule, VehicleColorTypeRoutingModule]
})
export class VehicleColorTypeModule { }
