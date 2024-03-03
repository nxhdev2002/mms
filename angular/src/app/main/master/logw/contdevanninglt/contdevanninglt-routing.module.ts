import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContDevanningLTComponent } from './contdevanninglt.component';

const routes: Routes = [{
    path: '',
    component: ContDevanningLTComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContDevanningLTRoutingModule {}
