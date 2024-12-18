import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { MaterialTypeComponent } from './materialtype.component';

const routes: Routes = [{
    path: '',
    component: MaterialTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MaterialTypeRoutingModule {}
