import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CallinglightstatusComponent } from './callinglightstatus.component';

const routes: Routes = [{
    path: '',
    component: CallinglightstatusComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CallinglightstatusRoutingModule {}
