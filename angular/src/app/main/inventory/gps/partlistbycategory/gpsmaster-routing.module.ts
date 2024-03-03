import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsMasterComponent } from './gpsmaster.component';


const routes: Routes = [{
    path: '',
    component: GpsMasterComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsMasterRoutingModule {}
