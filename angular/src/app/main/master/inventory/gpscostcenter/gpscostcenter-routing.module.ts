import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsCostCenterComponent } from './gpscostcenter.component';

const routes: Routes = [{
    path: '',
    component: GpsCostCenterComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsCostCenterRoutingModule {}
