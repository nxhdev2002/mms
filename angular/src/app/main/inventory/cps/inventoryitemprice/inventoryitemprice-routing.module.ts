import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InventoryItemPriceComponent } from './inventoryitemprice.component';


const routes: Routes = [{
    path: '',
    component: InventoryItemPriceComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InventoryItemPriceRoutingModule {}
