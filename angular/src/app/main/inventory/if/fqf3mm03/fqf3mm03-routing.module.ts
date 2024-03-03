import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FQF3MM03Component } from './fqf3mm03.component';

const routes: Routes = [{
    path: '',
    component: FQF3MM03Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FQF3MM03RoutingModule { }
