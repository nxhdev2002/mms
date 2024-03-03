import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DemDetDaysComponent } from './demdetdays.component';
import { CreateOrEditDemDetDaysModalComponent } from './create-or-edit-demdetdays-modal.component';
import { DemDetDaysRoutingModule } from './demdetdays-routing.module';
import { ImportDemDetDaysComponent } from './import-demdetdays.component';
import { ListErrorImportMstInvDemDetDaysComponent } from './list-error-import-demdetdays-modal.component';

@NgModule({
    declarations: [
        DemDetDaysComponent,
        ImportDemDetDaysComponent,
        CreateOrEditDemDetDaysModalComponent,
        ListErrorImportMstInvDemDetDaysComponent

    ],
    imports: [
        AppSharedModule, DemDetDaysRoutingModule]
})
export class DemDetDaysModule { }
