import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { TotalDelayFinalAsakaiComponent } from './totaldelayfinalasakai.component';

const routes: Routes = [{
    path: '',
    component: TotalDelayFinalAsakaiComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TotalDelayFinalAsakaiRoutingModule {}
