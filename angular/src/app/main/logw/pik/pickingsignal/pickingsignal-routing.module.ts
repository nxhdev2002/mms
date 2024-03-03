import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PickingSignalComponent } from './pickingsignal.component';

const routes: Routes = [{
    path: '',
    component: PickingSignalComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PickingSignalRoutingModule {}
