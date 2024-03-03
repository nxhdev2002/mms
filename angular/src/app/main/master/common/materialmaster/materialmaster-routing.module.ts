import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { MaterialMasterComponent } from './materialmaster.component';

const routes: Routes = [{
    path: '',
    component: MaterialMasterComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MaterialMasterRoutingModule {}
