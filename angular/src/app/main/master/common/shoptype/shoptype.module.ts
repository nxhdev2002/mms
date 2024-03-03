import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ShopTypeRoutingModule } from './shoptype-routing.module';
import { ShopTypeComponent } from './shoptype.component';
import { CreateOrEditShopTypeModalComponent } from './create-or-edit-shoptype-modal.component';

@NgModule({
    declarations: [
       ShopTypeComponent, 
        CreateOrEditShopTypeModalComponent
      
    ],
    imports: [
        AppSharedModule, ShopTypeRoutingModule]
})
export class ShopTypeModule {}
