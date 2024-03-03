import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PartDataComponent } from './partdata.component';

const routes: Routes = [{
    path: '',
    component: PartDataComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PartDataRoutingModule {}
