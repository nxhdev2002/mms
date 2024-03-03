import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { IhpStockPartComponent } from './ihpstockpart.component';
import { IhpStockPartRoutingModule } from './ihpstockpart-routing.module';
import { ViewIfIhpStockPartModalComponent } from './view-if-ihpstockpart-modal.component';

@NgModule({
    declarations: [
        IhpStockPartComponent,
        ViewIfIhpStockPartModalComponent
    ],
    imports: [
        AppSharedModule, IhpStockPartRoutingModule]
})
export class IhpStockPartModule { }
