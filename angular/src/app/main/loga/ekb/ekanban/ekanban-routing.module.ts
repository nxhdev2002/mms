import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EkanbanComponent } from './ekanban.component';


const routes: Routes = [{
    path: '',
    component: EkanbanComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EkanbanRoutingModule {}
