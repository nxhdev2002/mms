import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { DrmLocalPlanRoutingModule } from './drmlocalplan-routing.module';
import { DrmLocalPlanComponent } from './drmlocalplan.component';

@NgModule({
    declarations: [
       DrmLocalPlanComponent
      
    ],
    imports: [
        AppSharedModule, DrmLocalPlanRoutingModule]
})
export class DrmLocalPlanModule {}
