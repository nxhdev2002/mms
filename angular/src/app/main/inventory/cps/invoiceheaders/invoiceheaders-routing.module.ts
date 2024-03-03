import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvoiceHeadersComponent } from './invoiceheaders.component';

const routes: Routes = [{
    path: '',
    component: InvoiceHeadersComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvoiceHeadersRoutingModule {}
