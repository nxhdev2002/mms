import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProductTypeComponent } from './producttype.component';

const routes: Routes = [{
    path: '',
    component: ProductTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductTypeRoutingModule {}
