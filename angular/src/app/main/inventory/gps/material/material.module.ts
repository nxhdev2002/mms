import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { MaterialRoutingModule } from './material-routing.module';
import { InvGpsMaterialComponent } from './material.component';
import { CreateOrEditInvGpsMaterialModalComponent } from './create-or-edit-material-modal.component';
import { ImportGpsMaterialComponent } from './import-gps-material-modal.component';
import { ListErrorImportGpsMaterialExcelComponent } from './list-error-import-gpsmaterial-modal.component';
import { ViewHistoryMaterialModalComponent } from './history-material-modal.component';

@NgModule({
    declarations: [
        InvGpsMaterialComponent,
        CreateOrEditInvGpsMaterialModalComponent,
        ImportGpsMaterialComponent,
        ListErrorImportGpsMaterialExcelComponent,
        ViewHistoryMaterialModalComponent

    ],
    imports: [
        AppSharedModule, MaterialRoutingModule]
})
export class MaterialModule {}
