import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_LAYOUT]: LayoutComponent
};

@NgModule({
    declarations: [
       LayoutComponent,
    ],
    imports: [
        AppSharedModule, LayoutRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LayoutModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
