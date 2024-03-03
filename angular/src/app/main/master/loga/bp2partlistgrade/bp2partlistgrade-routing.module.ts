import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { Bp2PartListGradeComponent } from './bp2partlistgrade.component';

const routes: Routes = [{
    path: '',
    component: Bp2PartListGradeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class Bp2PartListGradeRoutingModule {}
