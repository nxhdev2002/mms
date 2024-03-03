import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EkbPartListComponent } from './ekbpartlist.component';

const routes: Routes = [{
    path: '',
    component: EkbPartListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EkbPartListRoutingModule {}
