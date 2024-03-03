import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ModelGradeComponent } from './modelgrade.component';

const routes: Routes = [{
    path: '',
    component: ModelGradeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ModelGradeRoutingModule {}
