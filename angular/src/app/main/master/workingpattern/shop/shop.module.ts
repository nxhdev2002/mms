import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { ShopRoutingModule } from './shop-routing.module';
import { ShopComponent } from './shop.component';
import { CreateOrEditShopModalComponent } from './create-or-edit-shop-modal.component';

// const tabcode_component_dict = {
//     [TABS.MASTER_WORKING_PATTERN_SHOP]: ShopComponent
// }

@NgModule({
    declarations: [
        ShopComponent,
        CreateOrEditShopModalComponent
    ],
    imports: [
        AppSharedModule, ShopRoutingModule],
        schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class ShopModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    // }
}
