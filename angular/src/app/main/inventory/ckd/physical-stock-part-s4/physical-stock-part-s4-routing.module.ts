import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PhysicalStockPartS4Component } from './physical-stock-part-s4.component';

const routes: Routes = [{
    path: '',
    component: PhysicalStockPartS4Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PhysicalStockPartS4RoutingModule {}
