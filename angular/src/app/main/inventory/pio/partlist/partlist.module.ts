import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
// import { PartListRoutingModule } from './partlist-routing.module';
import { PartListComponent } from './partlist.component';
import { PartListRoutingModule } from './partlist-routing.module';
import { ImportPartListComponent } from './import-partlist.component';
import { CreateOrEditPartListModalComponent } from './create-or-edit-partlist.component';
import { ViewHistoryPartListModalComponent } from './history-partlist-modal.component';
import { ListErrorImportInvPioPartListComponent } from './list-error-import-part-list-modal.component';
// import { CreateOrEditPartListModalComponent } from './create-or-edit-partlist-modal.component';

@NgModule({
    declarations: [
       PartListComponent,
       ImportPartListComponent,
       ViewHistoryPartListModalComponent,
       ListErrorImportInvPioPartListComponent,
       CreateOrEditPartListModalComponent
    ],
    imports: [
        AppSharedModule, PartListRoutingModule]
})
export class PartListModule {}
