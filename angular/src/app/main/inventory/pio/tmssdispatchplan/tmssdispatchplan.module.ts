import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { TmssDispatchPlanRoutingModule } from './tmssdispatchplan-routing.module';
import { TmssDispatchPlanComponent } from './tmssdispatchplan.component';
import { ViewHistoryInvTmssDispatchPlanComponent } from './history-tmssdispatchplan-modal.component';

@NgModule({
    declarations: [
       TmssDispatchPlanComponent,
       ViewHistoryInvTmssDispatchPlanComponent
       
    ],
    imports: [
        AppSharedModule, TmssDispatchPlanRoutingModule]
})
export class TmssDispatchPlanModule {}
