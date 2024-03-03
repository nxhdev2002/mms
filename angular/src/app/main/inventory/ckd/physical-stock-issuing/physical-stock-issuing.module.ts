import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { PhysicalStockIssuingComponent } from './physical-stock-issuing.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { PhysicalStockIssuingRoutingModule } from './physical-stock-issuing-routing.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { ViewPhysicalStockIssuingDetailModalComponent } from './view-modal.component';
import { ViewMaterialModalComponent } from './view-material-modal.component';
import { ViewLotDetailsComponent } from './view-lot-details-modal.component';
const tabcode_component_dict = {
    [TABS.INVENTORY_CKD_PHYSICALSTOCKISSUING]: PhysicalStockIssuingComponent
};
@NgModule({
    imports: [
        AppSharedModule, PhysicalStockIssuingRoutingModule,

    ],
    declarations: [
        PhysicalStockIssuingComponent,
        ViewMaterialModalComponent,
        ViewPhysicalStockIssuingDetailModalComponent,
        ViewLotDetailsComponent],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PhysicalStockIssuingModule {
    //   static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    //   }
}
