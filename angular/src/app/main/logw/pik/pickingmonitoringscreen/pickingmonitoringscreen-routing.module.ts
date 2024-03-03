import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PickingmonitoringScreenComponent } from './pickingmonitoringscreen.component';


const routes: Routes = [{
    path: '',
    component: PickingmonitoringScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PickingmonitoringScreenRoutingModule {}
