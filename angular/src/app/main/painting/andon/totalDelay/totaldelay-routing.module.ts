import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { TotalDelayComponent } from './totaldelay.component';

const routes: Routes = [{
    path: '',
    component: TotalDelayComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TotalDelayRoutingModule {}
