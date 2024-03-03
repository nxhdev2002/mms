import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StockComponent } from './stock.component';

const routes: Routes = [{
    path: '',
    component: StockComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockRoutingModule {}
