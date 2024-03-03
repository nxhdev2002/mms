import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ModelComponent } from './model.component';

const routes: Routes = [{
    path: '',
    component: ModelComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ModelRoutingModule {MasterDetailModule}
