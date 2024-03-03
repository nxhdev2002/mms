import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsTruckSupplierRoutingModule } from './gpstrucksupplier-routing.module';
import { GpsTruckSupplierComponent } from './gpstrucksupplier.component';
import { CreateOrEditGpsTruckSupplierModalComponent } from './create-or-edit-gpstrucksupplier-modal.component';

@NgModule({
    declarations: [
       GpsTruckSupplierComponent, 
        CreateOrEditGpsTruckSupplierModalComponent
      
    ],
    imports: [
        AppSharedModule, GpsTruckSupplierRoutingModule]
})
export class GpsTruckSupplierModule {}
