import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PhysicalStockPeriodRoutingModule } from './physical-stock-period-routing.module';
import { PhysicalStockPeriodComponent } from './physical-stock-period.component';
import { CloseAndOpenPhysicalPeriodModalComponent } from './close-and-open-physical-period-modal.component';
import { EditPhysicalPeriodModalComponent } from './edit-physical-period-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';
const tabcode_component_dict = {
    [TABS.INVENTORY_CKD_PHYSICALSTOCKPERIOD]: PhysicalStockPeriodComponent
  };
@NgModule({
    declarations: [
       PhysicalStockPeriodComponent,
       CloseAndOpenPhysicalPeriodModalComponent,
       EditPhysicalPeriodModalComponent
    ],
    imports: [
        AppSharedModule, PhysicalStockPeriodRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PhysicalStockPeriodModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    //   }
}
