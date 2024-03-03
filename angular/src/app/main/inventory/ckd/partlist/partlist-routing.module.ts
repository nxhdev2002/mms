import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CkdPartListComponent } from './partlist.component';

const routes: Routes = [{
    path: '',
    component: CkdPartListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PartListRoutingModule {}
