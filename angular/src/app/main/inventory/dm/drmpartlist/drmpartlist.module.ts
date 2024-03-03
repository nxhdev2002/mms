import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DrmPartListRoutingModule } from './drmpartlist-routing.module';
import { DrmPartListComponent } from './drmpartlist.component';
import { ImportDrmPartListComponent } from './import-drmpartlist.component';
import { ListErrorImportDrmPartListComponent } from './list-error-import-drmpartlist-modal.component';
import { CreateOrEditInvDrmPartListModalComponent } from './create-or-edit-drmpartlist-modal.component';
import { AddAssetDrmPartListModalComponent } from './add-asset-drmpartlist-modal.component';
import { ViewAsAssetModalComponent } from './view-asset-drmpartlist-modal.component';
import { ViewHistoryDrmPartlistModalComponent } from './history-drm-partlist-modal.component';
@NgModule({
    declarations: [
        DrmPartListComponent,
        ImportDrmPartListComponent,
        ListErrorImportDrmPartListComponent,
        CreateOrEditInvDrmPartListModalComponent,
        AddAssetDrmPartListModalComponent,
        ViewAsAssetModalComponent,
        ViewHistoryDrmPartlistModalComponent

    ],
    imports: [
        AppSharedModule, DrmPartListRoutingModule]
})
export class DrmPartListModule { }
