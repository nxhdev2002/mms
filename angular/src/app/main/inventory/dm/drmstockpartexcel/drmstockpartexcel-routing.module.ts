import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DrmStockPartExcelComponent } from './drmstockpartexcel.component';

const routes: Routes = [{
    path: '',
    component: DrmStockPartExcelComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DrmStockPartExcelRoutingModule { }
