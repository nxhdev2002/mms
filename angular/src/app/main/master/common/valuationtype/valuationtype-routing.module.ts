import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ValuationTypeComponent } from './valuationtype.component';

const routes: Routes = [{
    path: '',
    component: ValuationTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ValuationTypeRoutingModule {}
