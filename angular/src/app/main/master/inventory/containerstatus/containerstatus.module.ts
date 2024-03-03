import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerStatusRoutingModule } from './containerstatus-routing.module';
import { ContainerStatusComponent } from './containerstatus.component';

@NgModule({
    declarations: [
        ContainerStatusComponent


    ],
    imports: [
        AppSharedModule, ContainerStatusRoutingModule]
})
export class ContainerStatusModule { }
