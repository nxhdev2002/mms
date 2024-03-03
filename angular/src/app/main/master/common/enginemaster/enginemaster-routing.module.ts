import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EngineMasterComponent } from './enginemaster.component';

const routes: Routes = [{
    path: '',
    component: EngineMasterComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EngineMasterRoutingModule {}
