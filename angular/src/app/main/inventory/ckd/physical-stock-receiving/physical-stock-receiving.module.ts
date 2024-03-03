import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { PhysicalStockReceivingComponent } from './physical-stock-receiving.component';

import { TABS } from '@app/shared/constants/tab-keys';
import { ViewMaterialModalComponent } from './view-material-modal.component';
import { PhysicalStockReceivingRoutingModule } from './physical-stock-receiving-routing.module';
import { ViewPhysicalStockReceivingDetailModalComponent } from './view-physicalstockreceiving-detail-modal.component';
import { ViewLotDetailsComponent } from './view-lot-details-modal.component';

const tabcode_component_dict = {
    [TABS.INVENTORY_CKD_PHYSICALSTOCKRECEIVING]: PhysicalStockReceivingComponent
};
@NgModule({
    imports: [
        AppSharedModule,
        PhysicalStockReceivingRoutingModule,
    ],
    declarations: [PhysicalStockReceivingComponent,
        ViewMaterialModalComponent,
        ViewPhysicalStockReceivingDetailModalComponent,
        ViewLotDetailsComponent ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PhysicalStockReceivingModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    // }
}
