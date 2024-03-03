import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InventoryItemPriceRoutingModule } from './inventoryitemprice-routing.module';
import { InventoryItemPriceComponent } from './inventoryitemprice.component';

@NgModule({
    declarations: [
        InventoryItemPriceComponent,
    ],
    imports: [
        AppSharedModule, InventoryItemPriceRoutingModule]
})
export class InventoryItemPriceModule {}
