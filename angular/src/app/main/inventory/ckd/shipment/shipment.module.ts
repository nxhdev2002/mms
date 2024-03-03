import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ShipmentRoutingModule } from './shipment-routing.module';
import { ShipmentComponent } from './shipment.component';

@NgModule({
    declarations: [
       ShipmentComponent, 
      
    ],
    imports: [
        AppSharedModule, ShipmentRoutingModule]
})
export class ShipmentModule {}
