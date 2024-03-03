import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockReceivingTransactionComponent } from './stockreceivingtransaction.component';

const routes: Routes = [{
    path: '',
    component: StockReceivingTransactionComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockReceivingTransactionRoutingModule { }
