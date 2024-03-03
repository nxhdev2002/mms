import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { HrOrgStructureComponent } from './hrorgstructure.component';

const routes: Routes = [{
    path: '',
    component: HrOrgStructureComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HrOrgStructureRoutingModule {}
