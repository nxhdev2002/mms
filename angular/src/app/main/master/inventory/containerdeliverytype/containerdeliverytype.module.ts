import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerDeliveryTypeRoutingModule } from './containerdeliverytype-routing.module';
import { ContainerDeliveryTypeComponent } from './containerdeliverytype.component';

@NgModule({
    declarations: [
        ContainerDeliveryTypeComponent


    ],
    imports: [
        AppSharedModule, ContainerDeliveryTypeRoutingModule]
})
export class ContainerDeliveryTypeModule { }
