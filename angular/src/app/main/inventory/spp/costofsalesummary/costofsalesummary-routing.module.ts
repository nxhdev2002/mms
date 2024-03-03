import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CostOfSaleSummaryComponent } from './costofsalesummary.component';

const routes: Routes = [{
    path: '',
    component: CostOfSaleSummaryComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CostOfSaleSummaryRoutingModule { }
