import {NgModule} from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GpsWbsCCMappingComponent } from './gpswbsccmapping.component';


const routes: Routes = [{
    path: '',
    component: GpsWbsCCMappingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsWbsCCMappingRoutingModule {}
