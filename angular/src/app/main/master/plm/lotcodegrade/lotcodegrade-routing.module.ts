import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LotCodeGradeComponent } from './lotcodegrade.component';

const routes: Routes = [{
    path: '',
    component: LotCodeGradeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LotCodeGradeRoutingModule {}
