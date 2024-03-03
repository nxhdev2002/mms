import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LineEfficiencyDetailsComponent } from './lineefficiencydetails.component';

const routes: Routes = [{
    path: '',
    component: LineEfficiencyDetailsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LineEfficiencyDetailsRoutingModule {}
