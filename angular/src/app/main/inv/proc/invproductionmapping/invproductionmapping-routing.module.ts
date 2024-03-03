import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvProductionMappingComponent } from './invproductionmapping.component';

const routes: Routes = [{
    path: '',
    component: InvProductionMappingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class  InvProductionMappingRoutingModule {}
