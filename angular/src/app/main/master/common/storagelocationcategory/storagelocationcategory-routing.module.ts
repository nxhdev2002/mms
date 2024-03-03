import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { StorageLocationCategoryComponent } from './storagelocationcategory.component';

const routes: Routes = [{
    path: '',
    component: StorageLocationCategoryComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StorageLocationCategoryRoutingModule {}
