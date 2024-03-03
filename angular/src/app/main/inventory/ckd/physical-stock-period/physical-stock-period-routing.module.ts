import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PhysicalStockPeriodComponent } from './physical-stock-period.component';

const routes: Routes = [{
    path: '',
    component: PhysicalStockPeriodComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PhysicalStockPeriodRoutingModule {}
