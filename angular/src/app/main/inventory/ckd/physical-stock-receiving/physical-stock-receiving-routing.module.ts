import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PhysicalStockReceivingComponent } from './physical-stock-receiving.component';


const routes: Routes = [{
    path: '',
    component: PhysicalStockReceivingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PhysicalStockReceivingRoutingModule {}
