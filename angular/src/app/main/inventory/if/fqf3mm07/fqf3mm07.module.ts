import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FQF3MM07RoutingModule } from './fqf3mm07-routing.module';
import { FQF3MM07Component } from './fqf3mm07.component';
import { ViewFundCommmitmentItemDMComponent } from './view-fund-commitment-item-modal.component';
import { ViewLoggingResponseBudgetCheckComponent } from './view-logging-response-budget-check-modal.component';
import { ViewRequestBudgetCheckComponent } from './view-request-budget-check-modal';
import { ViewRequestCmmSelectedComponent } from './view-request-cmm-selected-modal';
import { ViewFqf3mm07ValidateModalComponent } from './view-fqf3mm07-validate-modal.component';
import { ViewFqf3mm07ValidateResultModalComponent } from './view-fqf3mm07-validate-result-modal.component';

@NgModule({
    declarations: [
        FQF3MM07Component,
        ViewFundCommmitmentItemDMComponent,
        ViewLoggingResponseBudgetCheckComponent,
        ViewRequestBudgetCheckComponent,
        ViewRequestCmmSelectedComponent,
        ViewFqf3mm07ValidateModalComponent,
        ViewFqf3mm07ValidateResultModalComponent
    ],
    imports: [
        AppSharedModule, FQF3MM07RoutingModule]
})
export class FQF3MM07Module { }
