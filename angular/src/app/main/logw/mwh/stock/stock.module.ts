import { AppSharedModule } from '@app/shared/app-shared.module';
import { NgModule } from '@angular/core';
import { StockRoutingModule } from './stock-routing.module';
import { StockComponent } from './stock.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_MWH_STOCKATWH]: StockComponent
};
@NgModule({
    declarations: [
        StockComponent
    ],
    imports: [
        AppSharedModule, StockRoutingModule]
})
export class StockModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
