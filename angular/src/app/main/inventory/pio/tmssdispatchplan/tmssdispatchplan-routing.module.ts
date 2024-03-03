import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { TmssDispatchPlanComponent } from './tmssdispatchplan.component';

const routes: Routes = [{
    path: '',
    component: TmssDispatchPlanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TmssDispatchPlanRoutingModule {}
