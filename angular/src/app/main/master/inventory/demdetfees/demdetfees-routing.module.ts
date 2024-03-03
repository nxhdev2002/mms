import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { DemDetFeesComponent } from './demdetfees.component';

const routes: Routes = [{
    path: '',
    component: DemDetFeesComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DemDetFeesRoutingModule {}
