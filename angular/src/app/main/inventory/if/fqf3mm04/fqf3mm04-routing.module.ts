import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { FQF3MM04Component } from './fqf3mm04.component';

const routes: Routes = [{
    path: '',
    component: FQF3MM04Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FQF3MM04RoutingModule {}
