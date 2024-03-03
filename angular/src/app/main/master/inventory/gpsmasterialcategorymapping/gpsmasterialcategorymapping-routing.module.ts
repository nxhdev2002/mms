import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { RouterModule, Routes } from '@angular/router';
import { GpsMasterialCategoryMappingComponent } from './gpsmasterialcategorymapping.component';


const routes: Routes = [{
    path: '',
    component: GpsMasterialCategoryMappingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsMasterialCategoryMappingRoutingModule {}
