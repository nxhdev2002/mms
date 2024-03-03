import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EkbUserComponent } from './ekbuser.component';

const routes: Routes = [{
    path: '',
    component: EkbUserComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EkbUserRoutingModule {}
