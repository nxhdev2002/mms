import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { InvGpsFindStockComponent } from './findstock.component';
import { InvGpsFindStockRoutingModule } from './findstock-routing.module';
import { ImportInvGpsFinstockComponent } from './import-finstock-modal.component';
import { ListErrorImportGpsFinStockComponent } from './list-error-import-finstock-modal.component';
import { ErrorImportFinstockModalComponent } from './error-import-finstock-modal.component';


@NgModule({
    declarations: [
        InvGpsFindStockComponent,
        ImportInvGpsFinstockComponent,
        ListErrorImportGpsFinStockComponent,
        ErrorImportFinstockModalComponent
    ],
    imports: [
        AppSharedModule, InvGpsFindStockRoutingModule]
})
export class InvGpsFindStockModule { }
