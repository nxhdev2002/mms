import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FQF3MM05Component } from './fqf3mm05.component';

const routes: Routes = [{
    path: '',
    component: FQF3MM05Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FQF3MM05RoutingModule { }
