import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GenBomDataComponent } from './genbomdata.component';

const routes: Routes = [{
    path: '',
    component: GenBomDataComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GenBomDataRoutingModule {}
