import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentRequestComponent } from './paymentrequest.component';

const routes: Routes = [{
    path: '',
    component: PaymentRequestComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaymentRequestRoutingModule { }
