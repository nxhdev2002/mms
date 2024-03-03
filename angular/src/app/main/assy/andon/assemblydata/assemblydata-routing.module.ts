import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { AssemblyDataComponent } from './assemblydata.component';

const routes: Routes = [{
    path: '',
    component: AssemblyDataComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AssemblyDataRoutingModule {}
