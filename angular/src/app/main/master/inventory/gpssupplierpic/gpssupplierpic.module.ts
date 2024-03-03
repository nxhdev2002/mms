import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsSupplierPicRoutingModule } from './gpssupplierpic-routing.module';
import { GpsSupplierPicComponent } from './gpssupplierpic.component';
import { CreateOrEditGpsSupplierPicModalComponent } from './create-or-edit-gpssupplierpic-modal.component';

@NgModule({
    declarations: [
       GpsSupplierPicComponent, 
        CreateOrEditGpsSupplierPicModalComponent
      
    ],
    imports: [
        AppSharedModule, GpsSupplierPicRoutingModule]
})
export class GpsSupplierPicModule {}
