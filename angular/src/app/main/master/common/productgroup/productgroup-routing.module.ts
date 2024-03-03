import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProductGroupComponent } from './productgroup.component';

const routes: Routes = [{
    path: '',
    component: ProductGroupComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductGroupRoutingModule {}
