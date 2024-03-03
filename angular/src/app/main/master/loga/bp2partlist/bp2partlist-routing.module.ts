import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { Bp2PartListComponent } from './bp2partlist.component';

const routes: Routes = [{
    path: '',
    component: Bp2PartListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class Bp2PartListRoutingModule {}
