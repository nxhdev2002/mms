import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { RouterModule, Routes } from '@angular/router';
import { GpsMasterialCategoryComponent } from './gpsmasterialcategory.component';


const routes: Routes = [{
    path: '',
    component: GpsMasterialCategoryComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsMasterialCategoryRoutingModule {}
