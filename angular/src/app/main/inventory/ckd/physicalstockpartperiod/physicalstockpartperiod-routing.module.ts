import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PhysicalStockPartPeriodComponent } from './physicalstockpartperiod.component';

const routes: Routes = [{
    path: '',
    component: PhysicalStockPartPeriodComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PhysicalStockPartPeriodRoutingModule {}
