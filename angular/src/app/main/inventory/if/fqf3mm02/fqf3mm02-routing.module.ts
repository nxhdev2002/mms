import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FQF3MM02Component } from './fqf3mm02.component';

const routes: Routes = [{
    path: '',
    component: FQF3MM02Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FQF3MM02RoutingModule { }
