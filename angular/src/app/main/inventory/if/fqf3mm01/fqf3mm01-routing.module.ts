import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FQF3MM01Component } from './fqf3mm01.component';

const routes: Routes = [{
    path: '',
    component: FQF3MM01Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FQF3MM01RoutingModule { }
