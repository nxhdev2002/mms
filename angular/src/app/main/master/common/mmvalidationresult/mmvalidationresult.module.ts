import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CmmMMValidationResultRoutingModule } from './mmvalidationresult-routing.module';
import { CmmMMValidationResultComponent } from './mmvalidationresult.component';
import { ViewHistoryMMValidationResultModalComponent } from './history-mmvalidationresult-modal.component';

@NgModule({
    declarations: [
        CmmMMValidationResultComponent,
        ViewHistoryMMValidationResultModalComponent

    ],
    imports: [
        AppSharedModule, CmmMMValidationResultRoutingModule]
})
export class CmmMMValidationResultModule { }
