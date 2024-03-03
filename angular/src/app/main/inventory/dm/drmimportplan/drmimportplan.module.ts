import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { DrmImportPlanRoutingModule } from './drmimportplan-routing.module';
import { DrmImportPlanComponent } from './drmimportplan.component';
import { CreateOrEditDrmImportPlanModalComponent } from './create-or-edit-drmimportplan-modal.component';
import { ImportDrmPlanComponent } from './import-plan.component';
import { ListErrorImportDrmImportPlanComponent } from './list-error-import-importplan-modal.component';

@NgModule({
    declarations: [
       DrmImportPlanComponent,
        CreateOrEditDrmImportPlanModalComponent,
        ImportDrmPlanComponent,
        ListErrorImportDrmImportPlanComponent

    ],
    imports: [
        AppSharedModule, DrmImportPlanRoutingModule]
})
export class DrmImportPlanModule {}
