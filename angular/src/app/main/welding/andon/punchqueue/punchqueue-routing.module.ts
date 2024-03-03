import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PunchQueueComponent } from './punchqueue.component';

const routes: Routes = [{
    path: '',
    component: PunchQueueComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PunchQueueRoutingModule {}
