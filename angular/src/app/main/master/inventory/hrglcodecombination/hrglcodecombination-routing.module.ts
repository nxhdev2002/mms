import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { HrGlCodeCombinationComponent } from './hrglcodecombination.component';

const routes: Routes = [{
    path: '',
    component: HrGlCodeCombinationComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HrGlCodeCombinationRoutingModule {}
