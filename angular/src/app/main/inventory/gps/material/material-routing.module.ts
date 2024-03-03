import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvGpsMaterialComponent } from './material.component';

const routes: Routes = [{
    path: '',
    component: InvGpsMaterialComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MaterialRoutingModule {}
