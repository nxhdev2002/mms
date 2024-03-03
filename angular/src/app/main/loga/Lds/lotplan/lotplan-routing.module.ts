import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LotplanComponent } from './lotplan.component';

const routes: Routes = [{
    path: '',
    component: LotplanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LotplanRoutingModule {}
