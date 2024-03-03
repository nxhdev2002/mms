import { CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import { LotUpackingAndonRoutingModule } from './lotupackingandon-routing.module';
import { LotUpackingAndonComponent } from './lotupackingandon.component';
import { AppCommonModule } from "@app/shared/common/app-common.module";
// import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
   // [TABS.LGW_MWH_LOGW_PUP_LOTUNPACKINGW1]: LotUpackingAndonComponent
};

@NgModule({
    declarations: [
        LotUpackingAndonComponent,
    ],
    imports: [
        LotUpackingAndonRoutingModule,
        AppCommonModule,
        CommonModule,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LotUpackingAndonModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    // }
}
