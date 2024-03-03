import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
// import {StockBalanceRoutingModule } from './stock-balance-routing.module';
import { StockBalanceComponent } from './stock-balance.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { ViewStockBalanceModalComponent } from './view-stockbalance-modal.component';
import { StockBalanceRoutingModule } from './stock-balance-routing.module';

const tabcode_component_dict = {
    [TABS.INVENTORY_CKD_STOCKBALANCE]: StockBalanceComponent
  };

@NgModule({
    declarations: [
        StockBalanceComponent ,
        ViewStockBalanceModalComponent
    ],
    imports: [
        AppSharedModule, StockBalanceRoutingModule]
})
export class StockBalanceModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    //   }
}
