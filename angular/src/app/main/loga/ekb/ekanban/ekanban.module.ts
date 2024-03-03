import {AppSharedModule} from '@app/shared/app-shared.module';
import { EkanbanRoutingModule } from './ekanban-routing.module';
import { EkanbanComponent } from './ekanban.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';

const tabcode_component_dict = {
    [TABS.LOGA_EKB_EKANBAN]: EkanbanComponent
};

@NgModule({
    declarations: [
        EkanbanComponent,
    ],
    imports: [
        EkanbanRoutingModule,AppSharedModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class EkanbanModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}



