import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProductionPlanComponent } from './productionplan.component';

const routes: Routes = [{
    path: '',
    component: ProductionPlanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductionPlanRoutingModule {}
