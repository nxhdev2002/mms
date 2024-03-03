import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BarScanInfoRoutingModule } from './barscaninfo-routing.module';
import { BarScanInfoComponent } from './barscaninfo.component';
import { CreateOrEditBarScanInfoModalComponent } from './create-or-edit-barscaninfo-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LOGA_BARSCANINFO]: BarScanInfoComponent
};

@NgModule({
    declarations: [
       BarScanInfoComponent,
        CreateOrEditBarScanInfoModalComponent
    ],
    imports: [
        AppSharedModule, BarScanInfoRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class BarScanInfoModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
