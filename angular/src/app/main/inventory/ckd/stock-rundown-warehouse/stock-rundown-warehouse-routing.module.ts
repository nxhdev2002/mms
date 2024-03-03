import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StockRundownWearehouseComponent } from './stock-rundown-warehouse.component';

const routes: Routes = [{
    path: '',
    component: StockRundownWearehouseComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockRundownWearehouseRoutingModule {}
