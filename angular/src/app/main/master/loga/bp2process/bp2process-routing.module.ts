import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { Bp2ProcessComponent } from './bp2process.component';

const routes: Routes = [{
    path: '',
    component: Bp2ProcessComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class Bp2ProcessRoutingModule {}
