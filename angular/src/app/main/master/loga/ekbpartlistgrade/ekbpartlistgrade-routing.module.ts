import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EkbPartListGradeComponent } from './ekbpartlistgrade.component';

const routes: Routes = [{
    path: '',
    component: EkbPartListGradeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EkbPartListGradeRoutingModule {}
