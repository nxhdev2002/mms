import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { RobbingLaneComponent } from './robbinglane.component';

const routes: Routes = [{
    path: '',
    component: RobbingLaneComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RobbingLaneRoutingModule {}
