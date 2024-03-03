import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsCategoryComponent } from './gpscategory.component';

const routes: Routes = [{
    path: '',
    component: GpsCategoryComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsCategoryRoutingModule {}
