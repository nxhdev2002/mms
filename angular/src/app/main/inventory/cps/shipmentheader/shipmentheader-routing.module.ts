import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ShipmentHeaderComponent } from './shipmentheader.component';


const routes: Routes = [{
    path: '',
    component: ShipmentHeaderComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ShipmentHeaderRoutingModule {MasterDetailModule}
