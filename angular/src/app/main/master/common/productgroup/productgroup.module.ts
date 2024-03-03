import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ProductGroupRoutingModule } from './productgroup-routing.module';
import { ProductGroupComponent } from './productgroup.component';
import { CreateOrEditProductGropupModalComponent } from './create-or-edit-productgroup-modal.component';


@NgModule({
    declarations: [
       ProductGroupComponent, 
       CreateOrEditProductGropupModalComponent,
      
    ],
    imports: [
        AppSharedModule, ProductGroupRoutingModule]
})
export class ProductGroupModule {}
