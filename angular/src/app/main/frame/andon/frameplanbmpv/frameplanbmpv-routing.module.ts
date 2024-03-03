import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FramePlanBMPVComponent } from './frameplanbmpv.component';

const routes: Routes = [{
    path: '',
    component: FramePlanBMPVComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FramePlanBMPVRoutingModule { }
