import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BarUserComponent } from './baruser.component';

const routes: Routes = [{
    path: '',
    component: BarUserComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BarUserRoutingModule {}
