import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PartRobbingComponent } from './partrobbing.component';

const routes: Routes = [{
    path: '',
    component: PartRobbingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PartRobbingRoutingModule {}
