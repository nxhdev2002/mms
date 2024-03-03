import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContainerTransitPortPlanComponent } from './containertransitportplan.component';

const routes: Routes = [{
    path: '',
    component: ContainerTransitPortPlanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerTransitPortPlanRoutingModule {}
