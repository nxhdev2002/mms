import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PhysicalStockPartComponent } from './physical-stock-part.component';

const routes: Routes = [{
    path: '',
    component: PhysicalStockPartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PhysicalStockPartRoutingModule {}
