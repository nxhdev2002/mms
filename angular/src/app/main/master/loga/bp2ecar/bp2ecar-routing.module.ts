import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { Bp2EcarComponent } from './bp2ecar.component';

const routes: Routes = [{
    path: '',
    component: Bp2EcarComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class Bp2EcarRoutingModule {}
