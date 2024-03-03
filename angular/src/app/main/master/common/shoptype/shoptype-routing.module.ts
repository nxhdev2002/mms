import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ShopTypeComponent } from './shoptype.component';

const routes: Routes = [{
    path: '',
    component: ShopTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ShopTypeRoutingModule {}
