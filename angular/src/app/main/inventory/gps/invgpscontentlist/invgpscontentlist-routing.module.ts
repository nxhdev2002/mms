import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvGpsContentListComponent } from './invgpscontentlist.component';

const routes: Routes = [{
    path: '',
    component: InvGpsContentListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvGpsContentListRoutingModule {}
