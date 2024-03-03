
import {AppSharedModule} from '@app/shared/app-shared.module';

import { PartListInlRoutingModule } from './partlistinl-routing.module';
import { PioPartListInlComponent } from './partlistinl.component';
import { ViewValidatePartListInlModalComponent } from './view-validate-partlistinl-modal.component';
import { EditPartGradeInlModalComponent } from './edit-partgradeinl-modal.component';
import { EditPartListInlModalComponent } from './edit-partlistinl-modal.component';
import { CreatePartListInlModalComponent } from './create-partlistinl-modal.component';
import { NgModule } from '@angular/core';

import { ListErrorImportInvPioPartListComponent } from './list-error-import-part-list-modal.component';
import { ImportInvPioPartListComponent } from './import-partlistinl.component';
import { ViewHistoryPartListInlModalComponent } from './history-partlistinl-modal.component';
import { EciPartGradeInlModalComponent } from './eci-partgradeinl-modal.component';



@NgModule({
    declarations: [CreatePartListInlModalComponent,
        PioPartListInlComponent,
        ViewValidatePartListInlModalComponent,
        EditPartListInlModalComponent,
        EditPartGradeInlModalComponent,
        ImportInvPioPartListComponent,
        ListErrorImportInvPioPartListComponent,
        ViewHistoryPartListInlModalComponent,
        EciPartGradeInlModalComponent

    ],
    imports: [
        AppSharedModule, PartListInlRoutingModule]
})
export class PartListInlModule {}
