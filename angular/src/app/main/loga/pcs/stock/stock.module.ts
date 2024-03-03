import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StockRoutingModule } from './stock-routing.module';
import { StockComponent } from './stock.component';
import { CreateOrEditStockModalComponent } from './create-or-edit-stock-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LOGA_PCS_STOCK]: StockComponent
};

@NgModule({
    declarations: [
       StockComponent,
        CreateOrEditStockModalComponent
    ],
    imports: [
        AppSharedModule, StockRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class StockModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}

