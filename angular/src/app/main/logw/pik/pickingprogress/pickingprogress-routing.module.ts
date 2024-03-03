import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PickingProgressComponent } from './pickingprogress.component';

const routes: Routes = [{
    path: '',
    component: PickingProgressComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PickingProgressRoutingModule {}
