import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PickingTabletComponent } from './pickingtablet.component';

const routes: Routes = [{
    path: '',
    component: PickingTabletComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PickingTabletRoutingModule {}
