import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockRundownComponent } from './stockrundown.component';

const routes: Routes = [{
    path: '',
    component: StockRundownComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockRundownRoutingModule { }
