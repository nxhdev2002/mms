import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { OutWipStockComponent } from './outwipstock.component';

const routes: Routes = [{
    path: '',
    component: OutWipStockComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OutWipStockRoutingModule {}

