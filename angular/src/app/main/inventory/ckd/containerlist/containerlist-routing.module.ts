import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContainerListComponent } from './containerlist.component';

const routes: Routes = [{
    path: '',
    component: ContainerListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerListRoutingModule {}
