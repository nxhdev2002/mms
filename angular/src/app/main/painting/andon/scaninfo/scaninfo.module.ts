import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { ScanInfoRoutingModule } from './scaninfo-routing.module';
import { ScanInfoComponent } from './scaninfo.component';

const tabcode_component_dict = {
    [TABS.PTA_ADO_SCANINFO]: ScanInfoComponent
}

@NgModule({
    declarations: [
       ScanInfoComponent,
    ],
    imports: [
        AppSharedModule, ScanInfoRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ScanInfoModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
