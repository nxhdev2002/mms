import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { APlanShiftBaseComponent } from './aplanshiftbase.component';

const routes: Routes = [{
    path: '',
    component: APlanShiftBaseComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class APlanShiftBaseRoutingModule {}
