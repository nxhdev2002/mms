import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { MATCustomsDeclareComponent } from './matcustomdeclare.component';

const routes: Routes = [{
    path: '',
    component: MATCustomsDeclareComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MATCustomsDeclareRoutingModule {MasterDetailModule}
