import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IssuingsComponent } from './issuings.component';

const routes: Routes = [{
    path: '',
    component: IssuingsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class IssuingsRoutingModule { }
