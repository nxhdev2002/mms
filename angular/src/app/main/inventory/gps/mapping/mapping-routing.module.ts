import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvGpsMappingComponent } from './mapping.component';

const routes: Routes = [{
    path: '',
    component: InvGpsMappingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvGpsMappingRoutingModule {}
