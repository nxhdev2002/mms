import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PhysicalStockIssuingComponent } from './physical-stock-issuing.component';

const routes: Routes = [{
    path: '',
    component: PhysicalStockIssuingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PhysicalStockIssuingRoutingModule {}
