import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContainerStatusComponent } from './containerstatus.component';

const routes: Routes = [{
    path: '',
    component: ContainerStatusComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerStatusRoutingModule {}
