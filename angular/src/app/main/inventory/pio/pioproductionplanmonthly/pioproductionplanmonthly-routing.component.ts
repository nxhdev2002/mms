import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PioProductionPlanMonthlyComponent } from './pioproductionplanmonthly.component';

const routes: Routes = [{
    path: '',
    component: PioProductionPlanMonthlyComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PioProductionPlanMonthlyRoutingModule {}
