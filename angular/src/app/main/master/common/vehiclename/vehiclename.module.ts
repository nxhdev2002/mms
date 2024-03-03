import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { VehicleNameRoutingModule } from './vehiclename-routing.module';
import { VehicleNameComponent } from './vehiclename.component';
import { viewVehicleNameModalComponent } from './view-vehiclename-modal.component';

@NgModule({
    declarations: [
        viewVehicleNameModalComponent,
        VehicleNameComponent


    ],
    imports: [
        AppSharedModule, VehicleNameRoutingModule]
})
export class VehicleNameModule { }
