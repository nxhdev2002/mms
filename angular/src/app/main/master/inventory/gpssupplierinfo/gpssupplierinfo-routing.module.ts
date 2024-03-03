import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsSupplierInfoComponent } from './gpssupplierinfo.component';

const routes: Routes = [{
    path: '',
    component: GpsSupplierInfoComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsSupplierInfoRoutingModule {}
