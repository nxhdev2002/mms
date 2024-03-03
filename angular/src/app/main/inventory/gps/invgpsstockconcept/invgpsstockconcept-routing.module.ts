import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvGpsStockConceptComponent } from './invgpsstockconcept.component';

const routes: Routes = [{
    path: '',
    component: InvGpsStockConceptComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvGpsStockConceptRoutingModule {}
