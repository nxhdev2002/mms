import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { WeldingPlanComponent } from './weldingplan.component';

const routes: Routes = [{
    path: '',
    component: WeldingPlanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WeldingPlanRoutingModule {}
