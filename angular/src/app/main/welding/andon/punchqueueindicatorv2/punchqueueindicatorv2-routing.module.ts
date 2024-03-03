import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { Punchqueueindicatorv2Component } from './punchqueueindicatorv2.component';

const routes: Routes = [{
    path: '',
    component: Punchqueueindicatorv2Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class Punchqueueindicatorv2RoutingModule {}
