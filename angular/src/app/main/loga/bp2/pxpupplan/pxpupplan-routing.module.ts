


import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PxPUpPlanComponent } from './pxpupplan.component';


const routes: Routes = [{
    path: '',
    component: PxPUpPlanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PxPUpPlanRoutingModule {}
