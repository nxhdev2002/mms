import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvGpsDailyOrderComponent } from './invgpsdailyorder.component';

const routes: Routes = [{
    path: '',
    component: InvGpsDailyOrderComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvGpsDailyOrderRoutingModule {}
