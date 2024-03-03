import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { AssemblyDataScreenComponent } from './assemblydatascreen.component';


const routes: Routes = [{
    path: '',
    component: AssemblyDataScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AssemblyDataScreenRoutingModule {}
