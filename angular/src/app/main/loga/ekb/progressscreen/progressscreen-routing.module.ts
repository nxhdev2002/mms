import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProgressScreenComponent } from './progressscreen.component';

const routes: Routes = [{
    path: '',
    component: ProgressScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class ProgressScreenRoutingModule {}
