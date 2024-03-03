import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { AinplanComponent } from './ainplan.component';

const routes: Routes = [{
    path: '',
    component: AinplanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AinplanRoutingModule {}
