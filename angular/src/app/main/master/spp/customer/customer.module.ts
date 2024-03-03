import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerComponent } from './customer.component';
// import { CreateOrEditCustomerModalComponent } from './create-or-edit-customer-modal.component';

@NgModule({
    declarations: [
       CustomerComponent, 
        // CreateOrEditCustomerModalComponent
      
    ],
    imports: [
        AppSharedModule, CustomerRoutingModule]
})
export class CustomerModule {}
