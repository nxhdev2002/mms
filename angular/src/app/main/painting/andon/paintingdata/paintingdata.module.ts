import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { PaintingDataRoutingModule } from './paintingdata-routing.module';
import { PaintingDataComponent } from './paintingdata.component';

const tabcode_component_dict = {
    [TABS.PTA_ADO_PAINTINGDATA]: PaintingDataComponent
}


@NgModule({
    declarations: [
       PaintingDataComponent
    ],
    imports: [
        AppSharedModule, PaintingDataRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PaintingDataModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
