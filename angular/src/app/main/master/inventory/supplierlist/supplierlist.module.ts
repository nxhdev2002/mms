import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { SupplierListRoutingModule } from './supplierlist-routing.module';
import { SupplierListComponent } from './supplierlist.component';
import { ViewSupplierListModalComponent } from './view-modal-supplierlist.component';

@NgModule({
    declarations: [
       SupplierListComponent,
       ViewSupplierListModalComponent
    ],
    imports: [
        AppSharedModule, SupplierListRoutingModule]
})
export class SupplierListModule {}
