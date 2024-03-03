import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EkbProcessComponent } from './ekbprocess.component';

const routes: Routes = [{
    path: '',
    component: EkbProcessComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EkbProcessRoutingModule {}
