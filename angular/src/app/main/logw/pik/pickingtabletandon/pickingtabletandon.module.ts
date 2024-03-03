import { CUSTOM_ELEMENTS_SCHEMA,NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';



//import customer
import { AppCommonModule } from "@app/shared/common/app-common.module";
import { PickingTabletAndonRoutingModule } from './pickingtabletandon-routing.module';
import { PickingTabletAndonComponent } from './pickingtabletandon.component';
import { PopupUnpackComponent } from './popup-unpack.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { AppSharedModule } from '@app/shared/app-shared.module';

const tabcode_component_dict = {
  //  [TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETUBW1]: PickingTabletAndonComponent
};
@NgModule({
    declarations: [
        PickingTabletAndonComponent,
        PopupUnpackComponent
    ],
    imports: [
        CommonModule,
        AppCommonModule,
        PickingTabletAndonRoutingModule,
        AppSharedModule
    ],
    exports:[
        PickingTabletAndonComponent,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PickingTabletAndonModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
