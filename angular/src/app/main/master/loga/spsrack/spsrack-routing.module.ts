import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { SpsRackComponent } from './spsrack.component';

const routes: Routes = [{
    path: '',
    component: SpsRackComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SpsRackRoutingModule {}
