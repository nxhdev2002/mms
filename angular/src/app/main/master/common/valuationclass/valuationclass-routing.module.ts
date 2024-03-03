import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ValuationClassComponent } from './valuationclass.component';

const routes: Routes = [{
    path: '',
    component: ValuationClassComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ValuationClassRoutingModule {}
