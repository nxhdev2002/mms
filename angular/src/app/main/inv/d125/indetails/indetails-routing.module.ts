import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InDetailsComponent } from './indetails.component';

const routes: Routes = [{
    path: '',
    component: InDetailsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InDetailsRoutingModule {}
