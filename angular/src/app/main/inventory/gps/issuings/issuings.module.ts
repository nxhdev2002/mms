import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { IssuingsRoutingModule } from './issuings-routing.module';
import { IssuingsComponent } from './issuings.component';
import { CreateItemIssuingsModalComponent } from './create-item-issuings-modal.component';
import { ImportInvGpsIssuingsComponent } from './import-issuings-modal.component';
import { ListErrorImportGpsIssuingsComponent } from './list-error-import-issuings-modal.component';
import { CreateDocumentIssuingsModalComponent } from './create-document-issuings-modal.component';
import { ViewLoggingResponseBudgetCheckComponent } from './view-logging-response-budget-check-modal.component';
import { ViewFundCommmitmentItemDMComponent } from './view-fund-commitment-item-modal.component';
import { ViewRequestCmmSelectedComponent } from './view-request-cmm-selected-modal';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { ViewRequestBudgetCheckComponent } from './view-request-budget-check-modal';
import { ViewHistoryIssuingsModalComponent } from './history-issuings-modal.component';
 

@NgModule({
  imports: [
    AppSharedModule,
    IssuingsRoutingModule,

  ],
  declarations: [
    IssuingsComponent,
    CreateDocumentIssuingsModalComponent,
    CreateItemIssuingsModalComponent,
    ImportInvGpsIssuingsComponent,
    ListErrorImportGpsIssuingsComponent,
    ViewLoggingResponseBudgetCheckComponent,
    ViewFundCommmitmentItemDMComponent,
    ViewRequestCmmSelectedComponent,
    ConfirmDialogComponent,
    ViewRequestBudgetCheckComponent,
    ViewHistoryIssuingsModalComponent
   ]
})
export class IssuingsModule { }
