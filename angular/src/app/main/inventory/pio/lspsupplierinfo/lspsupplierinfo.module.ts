import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { LspSupplierInfoComponent } from './lspsupplierinfo.component';
import { LspSupplierInfoRoutingModule } from './lspsupplierinfo-routing.module';
import { CreateOrEditLspSupplierInfoModalComponent } from './create-or-edit-lspsupplierinfo-modal.component';
// import { CreateOrEditCpsSuppliersModalComponent } from './create-or-edit-cpssuppliers-modal.component';

@NgModule({
    declarations: [
        LspSupplierInfoComponent, 
        CreateOrEditLspSupplierInfoModalComponent
      
    ],
    imports: [
        AppSharedModule, LspSupplierInfoRoutingModule]
})
export class LspSupplierInfoModule {}
