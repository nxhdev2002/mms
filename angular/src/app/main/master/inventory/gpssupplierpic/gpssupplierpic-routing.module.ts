import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsSupplierPicComponent } from './gpssupplierpic.component';

const routes: Routes = [{
    path: '',
    component: GpsSupplierPicComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsSupplierPicRoutingModule {}
