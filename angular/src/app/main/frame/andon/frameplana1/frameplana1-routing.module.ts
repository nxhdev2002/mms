import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FramePlanA1Component } from './frameplana1.component';

const routes: Routes = [{
    path: '',
    component: FramePlanA1Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FramePlanA1RoutingModule { }
