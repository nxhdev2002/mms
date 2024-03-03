import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProductionPlanMonthlyComponent } from './productionplanmonthly.component';

const routes: Routes = [{
    path: '',
    component: ProductionPlanMonthlyComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductionPlanMonthlyRoutingModule {}
