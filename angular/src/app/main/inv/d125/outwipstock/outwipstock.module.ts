import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { OutWipStockRoutingModule } from './outwipstock-routing.module';
import { OutWipStockComponent } from './outwipstock.component';
import { RequestModalComponent } from './request-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.INV_D125_OUTWIPSTOCK]: OutWipStockComponent
};

@NgModule({
    declarations: [
       OutWipStockComponent,
       RequestModalComponent
    ],
    imports: [
        AppSharedModule, OutWipStockRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class OutWipStockModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}

