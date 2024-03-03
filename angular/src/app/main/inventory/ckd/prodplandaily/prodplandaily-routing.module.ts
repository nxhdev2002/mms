import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProdPlanDailyComponent } from './prodplandaily.component';

const routes: Routes = [{
    path: '',
    component: ProdPlanDailyComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProdPlanDailyRoutingModule {}
