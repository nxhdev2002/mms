import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {  PunchQueueIndicatorComponent } from './punchqueueindicator.component';



const routes: Routes = [{
    path: '',
    component: PunchQueueIndicatorComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PunchQueueIndicatorRoutingModule {}
