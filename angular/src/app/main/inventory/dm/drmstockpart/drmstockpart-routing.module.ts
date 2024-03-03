import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DrmStockPartComponent } from './drmstockpart.component';

const routes: Routes = [{
    path: '',
    component: DrmStockPartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DrmStockPartRoutingModule { }
