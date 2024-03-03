import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { HrPositionComponent } from './hrposition.component';

const routes: Routes = [{
    path: '',
    component: HrPositionComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HrPositionRoutingModule {}
