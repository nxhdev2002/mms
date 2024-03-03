import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { DrmImportPlanComponent } from './drmimportplan.component';

const routes: Routes = [{
    path: '',
    component: DrmImportPlanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DrmImportPlanRoutingModule {}
