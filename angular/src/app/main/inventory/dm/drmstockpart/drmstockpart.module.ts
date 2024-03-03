import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DrmStockPartRoutingModule } from './drmstockpart-routing.module';
import { DrmStockPartComponent } from './drmstockpart.component';
import { ImportDrmStockPartComponent } from './import-drmstockpart.component';
import { ListErrorImportDrmStockPartComponent } from './list-error-import-drmstockpart-modal.component';

@NgModule({
    declarations: [
        DrmStockPartComponent,
        ImportDrmStockPartComponent,
        ListErrorImportDrmStockPartComponent

    ],
    imports: [
        AppSharedModule, DrmStockPartRoutingModule]
})
export class DrmStockPartModule { }
