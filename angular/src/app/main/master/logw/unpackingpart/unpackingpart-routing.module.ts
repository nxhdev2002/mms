import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { UnpackingPartComponent } from './unpackingpart.component';

const routes: Routes = [{
    path: '',
    component: UnpackingPartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UnpackingPartRoutingModule {}
