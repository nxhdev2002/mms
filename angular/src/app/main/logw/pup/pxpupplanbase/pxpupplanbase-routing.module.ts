import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PxPUpPlanBaseComponent } from './pxpupplanbase.component';

const routes: Routes = [{
    path: '',
    component: PxPUpPlanBaseComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PxPUpPlanBaseRoutingModule {}
