import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FuelTypeRoutingModule } from './fueltype-routing.module';
import { FuelTypeComponent } from './fueltype.component';
import { viewFuelTypeModalComponent } from './view-fueltype-modal.component';

@NgModule({
    declarations: [
        FuelTypeComponent,
        viewFuelTypeModalComponent

    ],
    imports: [
        AppSharedModule, FuelTypeRoutingModule]
})
export class FuelTypeModule { }
