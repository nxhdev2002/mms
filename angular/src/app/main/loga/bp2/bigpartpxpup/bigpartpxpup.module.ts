import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BigPartPxpUpRoutingModule } from './bigpartpxpup-routing.module';
import { BigPartPxpUpComponent } from './bigpartpxpup.component';

@NgModule({
    declarations: [
        BigPartPxpUpComponent
    ],
    imports: [
        BigPartPxpUpRoutingModule,AppSharedModule]
})
export class PxPUpPlanModule {}



