import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { InvGpsMappingRoutingModule } from './mapping-routing.module';
import { InvGpsMappingComponent } from './mapping.component';

@NgModule({
    declarations: [
        InvGpsMappingComponent
    ],
    imports: [
        AppSharedModule, InvGpsMappingRoutingModule]
})
export class InvGpsMappingModule { }
