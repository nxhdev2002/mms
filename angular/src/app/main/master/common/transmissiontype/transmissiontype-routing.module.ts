import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { TransmissionTypeComponent } from './transmissiontype.component';

const routes: Routes = [{
    path: '',
    component: TransmissionTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TransmissionTypeRoutingModule {}
