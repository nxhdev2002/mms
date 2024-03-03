import { CUSTOM_ELEMENTS_SCHEMA,NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';



//import customer
import { AppCommonModule } from "@app/shared/common/app-common.module";
import { PickingTabletAndonRouting2Module } from './pickingtabletandon2-routing.module';
import { PickingTabletAndon2Component } from './pickingtabletandon2.component';
import { PopupUnpack2Component } from './popup-unpack2.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { AppSharedModule } from '@app/shared/app-shared.module';

const tabcode_component_dict = {
   // [TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETUBW2]: PickingTabletAndon2Component
};
@NgModule({
    declarations: [
        PickingTabletAndon2Component,
        PopupUnpack2Component
    ],
    imports: [
        CommonModule,
        AppCommonModule,
        PickingTabletAndonRouting2Module,
        AppSharedModule,
    ],
    exports:[
        PickingTabletAndon2Component,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PickingTabletAndon2Module {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
