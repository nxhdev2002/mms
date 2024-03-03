import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EkanbanProgressScreenComponent } from './ekanbanprogressscreen.component';


const routes: Routes = [{
    path: '',
    component: EkanbanProgressScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EkanbanProgressScreenRoutingModule {}
