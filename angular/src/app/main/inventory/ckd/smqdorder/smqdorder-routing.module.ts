import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { SmqdOrderComponent } from './smqdorder.component';

const routes: Routes = [{
    path: '',
    component: SmqdOrderComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SmqdOrderRoutingModule {}
