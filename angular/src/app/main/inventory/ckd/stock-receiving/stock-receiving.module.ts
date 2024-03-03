import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StockReceivingComponent } from './stock-receiving.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { ViewStockReceivingDetailModalComponent } from './view-stockreceiving-detail-modal.component';
import { ViewStockReceivingMaterialModalComponent } from './view-stockreceiving-material-modal.component';
import { ViewStockReceivingValidateModalComponent } from './view-stockreceiving-validate-modal.component';
import { StockReceivingRoutingModule } from './stock-receiving-routing.module';


const tabcode_component_dict = {
    [TABS.INVENTORY_CKD_STOCKRECEIVING]: StockReceivingComponent
  };
@NgModule({
    declarations: [
      StockReceivingComponent,
      ViewStockReceivingDetailModalComponent,
      ViewStockReceivingMaterialModalComponent,
      ViewStockReceivingValidateModalComponent
    ],
    imports: [
        AppSharedModule, StockReceivingRoutingModule]
})
export class StockReceivingModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    //   }
}
