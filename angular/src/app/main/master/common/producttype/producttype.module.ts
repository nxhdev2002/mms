import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ProductTypeRoutingModule } from './producttype-routing.module';
import { ProductTypeComponent } from './producttype.component';


@NgModule({
    declarations: [
       ProductTypeComponent, 
        
      
    ],
    imports: [
        AppSharedModule, ProductTypeRoutingModule]
})
export class ProductTypeModule {}
