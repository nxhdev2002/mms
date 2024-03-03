import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvHrEmployeeComponent } from './invhremployee.component';

const routes: Routes = [{
    path: '',
    component: InvHrEmployeeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvHrEmployeeRoutingModule {}
