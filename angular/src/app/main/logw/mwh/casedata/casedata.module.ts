import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CaseDataHistoryComponent } from './casedata-history-modal.component';
import { CaseDataRoutingModule } from './casedata-routing.module';
import { CaseDataComponent } from './casedata.component';

@NgModule({
    declarations: [
       CaseDataComponent,
       CaseDataHistoryComponent

    ],
    imports: [
        AppSharedModule, CaseDataRoutingModule]
})
export class CaseDataModule {}
