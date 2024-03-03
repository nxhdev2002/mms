import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BarProcessComponent } from './barprocess.component';

const routes: Routes = [{
    path: '',
    component: BarProcessComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BarProcessRoutingModule {}
