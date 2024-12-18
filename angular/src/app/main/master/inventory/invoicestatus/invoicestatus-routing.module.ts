import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvoiceStatusComponent } from './invoicestatus.component';

const routes: Routes = [{
    path: '',
    component: InvoiceStatusComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvoiceStatusRoutingModule {}
