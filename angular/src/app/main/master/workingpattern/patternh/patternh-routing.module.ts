import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatternHComponent } from './patternh.component';

const routes: Routes = [{
    path: '',
    component: PatternHComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PatternHRoutingModule { }
