import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CostComponent } from './cost.component';

const routes: Routes = [{
    path: '',
    component: CostComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CostRoutingModule { }
