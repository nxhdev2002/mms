import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WorkingTimeComponent } from './workingtime.component';

const routes: Routes = [{
    path: '',
    component: WorkingTimeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WorkingTimeRoutingModule { }
