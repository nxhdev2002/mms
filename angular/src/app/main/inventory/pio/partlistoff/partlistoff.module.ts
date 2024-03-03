
import {AppSharedModule} from '@app/shared/app-shared.module';


import { NgModule } from '@angular/core';

import { PartListOffRoutingModule } from './partlistoff-routing.module';
import { PioPartListOffComponent } from './partlistoff.component';
import { ViewValidatePartListOffModalComponent } from './view-validate-partlistoff-modal.component';
import { ViewHistoryPartListOffModalComponent } from './history-partlistoff-modal.component';
import { CreatePartListOffModalComponent } from './create-partlistoff-modal.component';
import { EditPartListOffModalComponent } from './edit-partlistoff-modal.component';
import { EditPartGradeOffModalComponent } from './edit-partgradeoff-modal.component';
import { ImportInvPioPartListOffComponent } from './import-partlistoff.component';
import { ListErrorImportInvPioPartListComponent } from './list-error-import-part-list-modal.component';
import { EciPartGradeOffModalComponent } from './eci-partgradeoff-modal.component';




@NgModule({
    declarations: [
        PioPartListOffComponent,
        ViewValidatePartListOffModalComponent,
        ViewHistoryPartListOffModalComponent,
        CreatePartListOffModalComponent,
        EditPartListOffModalComponent,
        EditPartGradeOffModalComponent,
        ImportInvPioPartListOffComponent,
        ListErrorImportInvPioPartListComponent,
        EciPartGradeOffModalComponent
    ],
    imports: [
        AppSharedModule, PartListOffRoutingModule]
})
export class PartListOffModule {}
