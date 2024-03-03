import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PickingTabletSetupComponent } from './pickingtabletsetup.component';

const routes: Routes = [{
    path: '',
    component: PickingTabletSetupComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PickingTabletSetupRoutingModule {}
