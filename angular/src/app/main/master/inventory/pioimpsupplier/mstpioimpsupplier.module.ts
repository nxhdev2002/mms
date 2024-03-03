import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PioImpSupplierRoutingModule } from './mstpioimpsupplier-routing.module';
import { PioImpSupplierComponent } from './mstpioimpsupplier.component';
import { CreateOrEditPioImpSupplierModalComponent } from './create-or-edit-pioimpsupplier-modal.component';

@NgModule({
    declarations: [
        PioImpSupplierComponent, 
        CreateOrEditPioImpSupplierModalComponent
      
    ],
    imports: [
        AppSharedModule, PioImpSupplierRoutingModule]
})
export class PioImpSupplierModule {}
