import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LineEfficiencyComponent } from './lineefficiency.component';

const routes: Routes = [{
    path: '',
    component: LineEfficiencyComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LineEfficiencyRoutingModule {}
