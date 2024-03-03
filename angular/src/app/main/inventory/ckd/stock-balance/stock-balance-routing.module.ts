import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StockBalanceComponent } from './stock-balance.component';


const routes: Routes = [{
    path: '',
    component: StockBalanceComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockBalanceRoutingModule {}
