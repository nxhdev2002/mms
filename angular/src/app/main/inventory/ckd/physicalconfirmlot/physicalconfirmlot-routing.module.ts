import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PhysicalConfirmLotComponent } from './physicalconfirmlot.component';

const routes: Routes = [{
    path: '',
    component: PhysicalConfirmLotComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PhysicalConfirmLotRoutingModule {}
