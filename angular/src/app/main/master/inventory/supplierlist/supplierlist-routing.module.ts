import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { SupplierListComponent } from './supplierlist.component';

const routes: Routes = [{
    path: '',
    component: SupplierListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SupplierListRoutingModule {}
