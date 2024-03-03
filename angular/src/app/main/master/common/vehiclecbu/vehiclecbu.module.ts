import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { VehicleCBURoutingModule } from './vehiclecbu-routing.module';
import { VehicleCBUComponent } from './vehiclecbu.component';
import { ValidateVehicleCBUModalComponent } from './validate-vehicleCBU-modal.component';
import { ViewMaterialByIdModalComponent } from './view-material-modal.component';
 
import { ViewHistoryVehicleCBUModalComponent } from './history-vehiclecbu-modal.component';
 
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
 

@NgModule({
    declarations: [
        ConfirmDialogComponent,
        VehicleCBUComponent,
        ValidateVehicleCBUModalComponent,
        ViewMaterialByIdModalComponent,
        ViewHistoryVehicleCBUModalComponent
    ],
    imports: [
        AppSharedModule, VehicleCBURoutingModule,
        ]
})
export class VehicleCBUModule { }
