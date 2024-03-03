import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { StockIssuingComponent } from './stock-issuing.component';
import { ViewStockIssuingDetailModalComponent } from './view-stockissuing-detail-modal.component';
import { ViewMaterialByIdModalComponent } from './view-material-modal.component';
import { ValidateIssuingModalComponent } from './validate-issuing-modal.component';
import { StockIssuingRoutingModule } from './stock-issuing-routing.module';
import { ViewTransferLocComponent } from './view-transfer-sloc-modal.component';
import { ViewHistoryStockIssuingModalComponent } from './history-stock-issuing-modal.component';

const tabcode_component_dict = {
    [TABS.INVENTORY_CKD_STOCKISSUSING]: StockIssuingComponent
  };
@NgModule({
    declarations: [
       StockIssuingComponent,
       ViewStockIssuingDetailModalComponent,
       ViewMaterialByIdModalComponent,
       ValidateIssuingModalComponent,
       ViewTransferLocComponent,
       ViewHistoryStockIssuingModalComponent

    ],
    imports: [
        AppSharedModule,StockIssuingRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class StockIssuingModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    //   }
}
