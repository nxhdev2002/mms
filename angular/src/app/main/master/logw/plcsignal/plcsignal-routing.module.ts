import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PlcsignalComponent } from './plcsignal.component';

const routes: Routes = [{
    path: '',
    component: PlcsignalComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PlcsignalRoutingModule {}
