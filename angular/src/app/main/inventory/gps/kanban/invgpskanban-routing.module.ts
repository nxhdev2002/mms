import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvGpsKanbanComponent } from './invgpskanban.component';

const routes: Routes = [{
    path: '',
    component: InvGpsKanbanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvGpsKanbanRoutingModule {}
