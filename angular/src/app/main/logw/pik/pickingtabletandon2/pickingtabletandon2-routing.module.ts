import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PickingTabletAndon2Component } from './pickingtabletandon2.component';

const routes: Routes = [{
    path: '',
    component: PickingTabletAndon2Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class PickingTabletAndonRouting2Module {}
