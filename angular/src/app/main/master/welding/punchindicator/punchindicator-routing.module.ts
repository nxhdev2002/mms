import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PunchIndicatorComponent } from './punchindicator.component';

const routes: Routes = [{
    path: '',
    component: PunchIndicatorComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PunchIndicatorRoutingModule {}
