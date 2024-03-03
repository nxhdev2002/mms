import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LotUpPlanComponent } from './lotupplan.component';

const routes: Routes = [{
    path: '',
    component: LotUpPlanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LotUpPlanRoutingModule {}
