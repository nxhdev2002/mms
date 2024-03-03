import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StockRundownShippingResultComponent } from './stock-rundown-shipping-result.component';

const routes: Routes = [{
    path: '',
    component: StockRundownShippingResultComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockRundownShippingResultRoutingModule {}
