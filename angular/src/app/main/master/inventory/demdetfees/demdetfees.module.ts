import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DemDetFeesComponent } from './demdetfees.component';
import { DemDetFeesRoutingModule } from './demdetfees-routing.module';
import { CreateOrEditDemDetFeesModalComponent } from './create-or-edit-demdetfees-modal.component';
import { ImportDemDetFeesComponent } from './import-demdetfees.component';
import { ListErrorImportMstInvDemDetFeesComponent } from './list-error-import-demdetfees-modal.component';

@NgModule({
    declarations: [
        DemDetFeesComponent,
        CreateOrEditDemDetFeesModalComponent,
        ImportDemDetFeesComponent,
        ListErrorImportMstInvDemDetFeesComponent
    ],
    imports: [
        AppSharedModule, DemDetFeesRoutingModule]
})
export class DemDetFeesModule { }
