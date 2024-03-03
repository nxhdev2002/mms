import { NgModule } from '@angular/core';
import { PartListComponent } from './partlist.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { PartListRoutingModule } from './partlist-routing.module';
import { ImportPartListComponent } from './import-partlist-modal.component';
import { ListErrorImportGpsPartListComponent } from './list-error-import-part-list-modal.component';
import { ViewGpsValidatePartListModalComponent } from './view-gps-validate-partlist-modal.component';

@NgModule({
  imports: [
    AppSharedModule,
    PartListRoutingModule,

  ],
  declarations: [PartListComponent,
    ImportPartListComponent,
    ListErrorImportGpsPartListComponent,
    ViewGpsValidatePartListModalComponent]
})
export class PartListModule { }
