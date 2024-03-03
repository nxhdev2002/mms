import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvGpsFindStockComponent } from './findstock.component';


const routes: Routes = [{
    path: '',
    component: InvGpsFindStockComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class  InvGpsFindStockRoutingModule {}
