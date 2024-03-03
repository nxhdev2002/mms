import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { RequestModalComponent } from './request-modal.component';
import { StockRoutingModule } from './stock-routing.module';
import { StockComponent } from './stock.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.INV_D125_STOCK]: StockComponent
};

@NgModule({
    declarations: [
       StockComponent,
       RequestModalComponent
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
