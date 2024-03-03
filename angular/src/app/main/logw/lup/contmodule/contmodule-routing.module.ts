import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContModuleComponent } from './contmodule.component';

const routes: Routes = [{
    path: '',
    component: ContModuleComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContModuleRoutingModule {}
