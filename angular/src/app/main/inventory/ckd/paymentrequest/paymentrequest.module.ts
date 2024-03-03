import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentRequestComponent } from './paymentrequest.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { PaymentRequestRoutingModule } from './paymentrequest-routing.module';

@NgModule({
  imports: [
    AppSharedModule, PaymentRequestRoutingModule
  ],
  declarations: [PaymentRequestComponent]
})
export class PaymentRequestModule { }
