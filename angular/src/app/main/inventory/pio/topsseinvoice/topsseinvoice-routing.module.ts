import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TopsseInvoiceComponent } from './topsseinvoice.component';

const routes: Routes = [{
    path: '',
    component: TopsseInvoiceComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TopsseInvoiceRoutingModule { }
