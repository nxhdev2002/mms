import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { WeldingPlanRoutingModule } from './weldingplan-routing.module';
import { WeldingPlanComponent } from './weldingplan.component';
import { CreateOrEditWeldingPlanModalComponent } from './create-or-edit-weldingplan-modal.component';
import { ImportWeldingPlanComponent } from './import-weldingplan-modal.component';
import { ListErrorImportWeldingPlanComponent } from './list-error-import-weldingplan-modal.component';
import { ViewWeldingPlanModalComponent } from './view-weldingplan-modal.component';
import { ViewHistoryWeldingPlanComponent } from './history-weldingplan-modal.component';

@NgModule({
    declarations: [
       WeldingPlanComponent,
        CreateOrEditWeldingPlanModalComponent,
        ImportWeldingPlanComponent,
        ListErrorImportWeldingPlanComponent,
        ViewWeldingPlanModalComponent,
        ViewHistoryWeldingPlanComponent

    ],
    imports: [
        AppSharedModule, WeldingPlanRoutingModule]
})
export class WeldingPlanModule {}
