import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PartListRoutingModule } from './partlist-routing.module';
import { CkdPartListComponent } from './partlist.component';
import { ImportInvCkdPartListComponent } from './import-partlist.component';
import { ListErrorImportInvCkdPartListComponent } from './list-error-import-part-list-modal.component';
import { CreatePartListModalComponent } from './create-partlist-modal.component';
import { EditPartGradeModalComponent } from './edit-partgrade-modal.component';
import { EditPartListModalComponent } from './edit-partlist-modal.component';
import { ViewValidatePartListModalComponent } from './view-validate-partlist-modal.component';
import { ViewHistoryPartListModalComponent } from './history-partlist-modal.component';
import { EciPartGradeModalComponent } from './eci-partgrade-modal.component';

import { ImportInvCkdPartListNewComponent } from './import-partlist-new.component';
import { ListErrorImportInvCkdPartlistNewComponent } from './list-error-import-partlist-new-modal.component';

import { ImportPartGradeComponent } from './import-partgrade.component';
import { ListErrorImportInvCkdPartGradeComponent } from './list-error-import-part-grade-modal.component';


@NgModule({
    declarations: [
        CkdPartListComponent,
       ImportInvCkdPartListComponent,
       ListErrorImportInvCkdPartListComponent,
       CreatePartListModalComponent,
       EditPartListModalComponent,
       EditPartGradeModalComponent,
       ViewValidatePartListModalComponent,
       ViewHistoryPartListModalComponent,
       EciPartGradeModalComponent,

       ImportInvCkdPartListNewComponent,
       ListErrorImportInvCkdPartlistNewComponent,
       ImportPartGradeComponent,
       ListErrorImportInvCkdPartGradeComponent
    ],
    imports: [
        AppSharedModule, PartListRoutingModule],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PartListModule {}
