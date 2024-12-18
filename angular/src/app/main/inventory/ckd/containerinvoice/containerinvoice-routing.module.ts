import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContainerInvoiceComponent } from './containerinvoice.component';

const routes: Routes = [{
    path: '',
    component: ContainerInvoiceComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerInvoiceRoutingModule {}
