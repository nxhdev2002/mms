import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatternDComponent } from './patternd.component';

const routes: Routes = [{
    path: '',
    component: PatternDComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PatternDRoutingModule { }
