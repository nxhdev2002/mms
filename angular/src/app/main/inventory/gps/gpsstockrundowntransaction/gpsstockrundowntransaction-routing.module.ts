import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsStockRundownTransactionComponent } from './gpsstockrundowntransaction.component';

const routes: Routes = [{
    path: '',
    component: GpsStockRundownTransactionComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsStockRundownTransactionRoutingModule {}
