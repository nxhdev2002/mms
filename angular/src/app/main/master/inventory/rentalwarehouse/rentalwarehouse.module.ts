import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { RentalWarehouseRoutingModule } from './rentalwarehouse-routing.module';
import { RentalWarehouseComponent } from './rentalwarehouse.component';
import { CreateOrEditMstInvCkdRentalWarehouseModalComponent } from './create-or-edit-rentalwarehouse-modal.component';

@NgModule({
    declarations: [
        RentalWarehouseComponent,
        CreateOrEditMstInvCkdRentalWarehouseModalComponent

    ],
    imports: [
        AppSharedModule, RentalWarehouseRoutingModule],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class RentalWarehouseModule { }
