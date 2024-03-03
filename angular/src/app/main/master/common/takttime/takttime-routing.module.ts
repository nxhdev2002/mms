import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { TakttimeComponent } from './takttime.component';

const routes: Routes = [{
    path: '',
    component: TakttimeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TakttimeRoutingModule {}
