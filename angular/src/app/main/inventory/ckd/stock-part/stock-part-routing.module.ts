import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StockPartComponent } from './stock-part.component';


const routes: Routes = [{
    path: '',
    component: StockPartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockPartRoutingModule {}
