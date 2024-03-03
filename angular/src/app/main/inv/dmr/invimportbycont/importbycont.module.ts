import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ImportByContRoutingModule } from './importbycont-routing.module';
import { ImportByContComponent } from './importbycont.component';
import { RequestModalComponent } from './request-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.INV_DMR_IMPORTBYCONT]: ImportByContComponent
};
@NgModule({
    declarations: [
       ImportByContComponent,
       RequestModalComponent
    ],
    imports: [
        AppSharedModule, ImportByContRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ImportByContModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}