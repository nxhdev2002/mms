import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ReceivingRoutingModule } from './receiving-routing.module';
import { ReceivingComponent } from './receiving.component';
import { CreateOrEditReceivingModalComponent } from './create-or-edit-receiving-modal.component';
import { ImportReceivingComponent } from './import-receiving.component';
import { ListErrorImportReceivingComponent } from './list-error-import-receiving-modal.component';

@NgModule({
    declarations: [
        ReceivingComponent,
        CreateOrEditReceivingModalComponent,
        ImportReceivingComponent,
        ListErrorImportReceivingComponent,


    ],
    imports: [
        AppSharedModule, ReceivingRoutingModule]
})
export class StockReceivingModule { }
