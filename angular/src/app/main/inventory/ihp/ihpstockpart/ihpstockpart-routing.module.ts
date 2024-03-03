import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IhpStockPartComponent } from './ihpstockpart.component';


const routes: Routes = [{
    path: '',
    component: IhpStockPartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class IhpStockPartRoutingModule { }
