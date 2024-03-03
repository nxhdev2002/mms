import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContListComponent } from './contlist.component';

const routes: Routes = [{
    path: '',
    component: ContListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContListRoutingModule {}
