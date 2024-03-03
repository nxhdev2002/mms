import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PhysicalStockPartRoutingModule } from './physical-stock-part-routing.module';
import { PhysicalStockPartComponent } from './physical-stock-part.component';
import { CreateOrEditPhysicalStockPartModalComponent } from './create-or-edit-physical-stock-part-modal.component';
import { ImportInvCkdPhysicalStockPartComponent } from './import-physical-stock-part.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { ListErrorImportInvCkdPhysicalStockPartComponent } from './list-error-import-physical-stock-part-modal.component';
import { ViewLotDetailsComponent } from './view-lot-details-modal.component';

const tabcode_component_dict = {
    [TABS.INVENTORY_CKD_PHYSICALSTOCKPART]: PhysicalStockPartComponent
  };

@NgModule({
    declarations: [
       PhysicalStockPartComponent,
        CreateOrEditPhysicalStockPartModalComponent,
        ImportInvCkdPhysicalStockPartComponent,
        ListErrorImportInvCkdPhysicalStockPartComponent,
        ViewLotDetailsComponent

    ],
    imports: [
        AppSharedModule, PhysicalStockPartRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PhysicalStockPartModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    //   }
}
