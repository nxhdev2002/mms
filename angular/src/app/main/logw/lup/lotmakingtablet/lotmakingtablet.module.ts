import { CUSTOM_ELEMENTS_SCHEMA,NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { LotMakingTabletRoutingModule } from './lotmakingtablet-routing.module';
import { LotMakingTabletComponent } from './lotmakingtablet.component';
import { LotMakingTabletModalComponent } from './lotmakingtablet-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LGW_MWH_LOGW_PUP_LOTMAKINGTABLET]: LotMakingTabletRoutingModule
};

@NgModule({
    imports: [
        LotMakingTabletRoutingModule,
        AppSharedModule

    ],
    declarations: [
        LotMakingTabletComponent,
        LotMakingTabletModalComponent,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LotMakingTabletModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
