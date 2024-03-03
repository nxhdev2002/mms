import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { RenbanModuleComponent } from './renbanmodule.component';

const routes: Routes = [{
    path: '',
    component: RenbanModuleComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RenbanModuleRoutingModule {}
