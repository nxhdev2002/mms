import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProductionMappingComponent } from './productionmapping.component';

const routes: Routes = [{
    path: '',
    component: ProductionMappingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductionMappingRoutingModule {}
