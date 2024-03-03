import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockTransactionComponent } from './stocktransaction.component';

const routes: Routes = [{
    path: '',
    component: StockTransactionComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockTransactionRoutingModule { }
