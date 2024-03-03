import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { IhpPartListRoutingModule } from './ihppartlist-routing.module';
import { IhpPartListComponent } from './ihppartlist.component';
import { ViewIhpValidatePartListModalComponent } from './view-ihp-validate-partlist-modal.component';
import { AddIhpPartListModalComponent } from './edit-partlist/add-ihppartlist-modal.component';
import { EditIhpPartGradeModalComponent } from './edit-partgrade/edit-ihppartgrade-modal.component';
import { EditIhpPartListModalComponent } from './edit-partlist/edit-ihppartlist-modal.component';
import { ViewHistoryPartListModalComponent } from './history-partlist-modal.component';

@NgModule({
    declarations: [
        IhpPartListComponent,
        ViewIhpValidatePartListModalComponent,
        AddIhpPartListModalComponent,
        EditIhpPartGradeModalComponent,
        EditIhpPartListModalComponent,
        ViewHistoryPartListModalComponent
    ],
    imports: [
        AppSharedModule, IhpPartListRoutingModule]
})
export class IhpPartListModule { }
