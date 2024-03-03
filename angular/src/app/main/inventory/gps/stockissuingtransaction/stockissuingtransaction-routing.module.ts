import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockIssuingTransactionComponent } from './stockissuingtransaction.component';

const routes: Routes = [{
    path: '',
    component: StockIssuingTransactionComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockIssuingTransactionRoutingModule { }
