import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { HrTitlesComponent } from './hrtitles.component';

const routes: Routes = [{
    path: '',
    component: HrTitlesComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HrTitlesRoutingModule {}
