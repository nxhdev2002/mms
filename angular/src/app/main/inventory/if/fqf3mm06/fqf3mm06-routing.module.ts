import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FQF3MM06Component } from './fqf3mm06.component';

const routes: Routes = [{
    path: '',
    component: FQF3MM06Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FQF3MM06RoutingModule { }
