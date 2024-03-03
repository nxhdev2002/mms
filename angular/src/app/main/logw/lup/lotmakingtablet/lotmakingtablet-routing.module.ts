import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LotMakingTabletComponent } from './lotmakingtablet.component';

const routes: Routes = [{
    path: '',
    component: LotMakingTabletComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LotMakingTabletRoutingModule {}
