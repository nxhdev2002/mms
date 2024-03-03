import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ShippingCompanyRoutingModule } from './shippingcompany-routing.module';
import { ShippingCompanyComponent } from './shippingcompany.component';

@NgModule({
    declarations: [
        ShippingCompanyComponent


    ],
    imports: [
        AppSharedModule, ShippingCompanyRoutingModule]
})
export class ShippingCompanyModule { }
