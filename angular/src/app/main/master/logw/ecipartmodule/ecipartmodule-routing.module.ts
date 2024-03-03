import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EciPartModuleComponent } from './ecipartmodule.component';

const routes: Routes = [{
    path: '',
    component: EciPartModuleComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EciPartModuleRoutingModule {}
