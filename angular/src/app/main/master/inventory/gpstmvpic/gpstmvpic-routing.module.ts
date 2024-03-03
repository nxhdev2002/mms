import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsTmvPicComponent } from './gpstmvpic.component';

const routes: Routes = [{
    path: '',
    component: GpsTmvPicComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsTmvPicRoutingModule {}
