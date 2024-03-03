import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ContainerDeliveryGateInRoutingModule } from './containerdeliverygatein-routing.module';
import { ContainerDeliveryGateInComponent } from './containerdeliverygatein.component';

@NgModule({
    declarations: [
       ContainerDeliveryGateInComponent, 
      
    ],
    imports: [
        AppSharedModule, ContainerDeliveryGateInRoutingModule]
})
export class ContainerDeliveryGateInModule {}
