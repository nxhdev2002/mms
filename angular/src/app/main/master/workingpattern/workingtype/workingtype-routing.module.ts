import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WorkingTypeComponent } from './workingtype.component';

const routes: Routes = [{
    path: '',
    component: WorkingTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WorkingTypeRoutingModule { }
