import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StockIssuingComponent } from './stockissuing.component';

const routes: Routes = [{
    path: '',
    component: StockIssuingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockIssuingRoutingModule {}
