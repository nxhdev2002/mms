import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { TripRoutingModule } from './trip-routing.module';
import { TripComponent } from './trip.component';
import { CreateOrEditTripModalComponent } from './create-or-edit-trip-modal.component';

@NgModule({
    declarations: [
       TripComponent,
        CreateOrEditTripModalComponent

    ],
    imports: [
        AppSharedModule, TripRoutingModule]
})
export class TripModule {}
