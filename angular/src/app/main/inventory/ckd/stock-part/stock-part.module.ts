import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
// import { TABS } from '@app/shared/constants/tab-keys';
import { StockPartComponent } from './stock-part.component';
import { ViewStockPartDetailModalComponent } from './view-stockpart-detail-modal.component';
import { ViewMaterialByIdModalComponent } from './view-material-modal.component';
import { CheckStockPartModalComponent } from './check-stock-part-modal.component';
import { StockPartRoutingModule } from './stock-part-routing.module';
import { EditStockPartModalComponent } from './edit-stock-part-modal.component';

// const tabcode_component_dict = {
//     [TABS.INVENTORY_CKD_STOCKPART]: StockPartComponent
//   };
@NgModule({
    declarations: [
       StockPartComponent,
       ViewStockPartDetailModalComponent,
       ViewMaterialByIdModalComponent,
       CheckStockPartModalComponent,
       EditStockPartModalComponent
    ],
    imports: [
        AppSharedModule, StockPartRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class StockPartModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    //   }
}

