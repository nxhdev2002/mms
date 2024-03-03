import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ModuleUnpackingAndonComponent } from './module-unpacking-andon-screen.component';

const routes: Routes = [{
    path: '',
    component: ModuleUnpackingAndonComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ModuleUnpackingAndonRoutingModule {}
