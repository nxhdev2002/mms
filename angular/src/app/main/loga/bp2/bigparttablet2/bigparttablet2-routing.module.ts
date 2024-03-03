import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BigPartTablet2Component } from './bigparttablet2.component';

const routes: Routes = [{
    path: '',
    component: BigPartTablet2Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class BigPartTablet2RoutingModule {}
