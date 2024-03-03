import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProgressDetailsComponent } from './progressdetails.component';

const routes: Routes = [{
    path: '',
    component: ProgressDetailsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProgressDetailsRoutingModule {}
