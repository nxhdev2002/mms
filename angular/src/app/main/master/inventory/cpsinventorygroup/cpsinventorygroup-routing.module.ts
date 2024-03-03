import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CpsInventoryGroupComponent } from './cpsinventorygroup.component';

const routes: Routes = [{
    path: '',
    component: CpsInventoryGroupComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CpsInventoryGroupRoutingModule {}
