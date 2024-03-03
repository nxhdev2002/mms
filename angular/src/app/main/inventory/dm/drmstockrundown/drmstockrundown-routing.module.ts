import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { DrmStockRundownComponent } from './drmstockrundown.component';

const routes: Routes = [{
    path: '',
    component: DrmStockRundownComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DrmStockRundownRoutingModule {}
