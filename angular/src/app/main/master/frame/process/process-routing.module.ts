import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProcessComponent } from './process.component';

const routes: Routes = [{
    path: '',
    component: ProcessComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProcessRoutingModule {}
