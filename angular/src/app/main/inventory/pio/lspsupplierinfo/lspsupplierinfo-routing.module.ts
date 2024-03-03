import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LspSupplierInfoComponent } from './lspsupplierinfo.component';

const routes: Routes = [{
    path: '',
    component: LspSupplierInfoComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LspSupplierInfoRoutingModule {}
