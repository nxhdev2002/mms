import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContainerDeliveryGateInComponent } from './containerdeliverygatein.component';

const routes: Routes = [{
    path: '',
    component: ContainerDeliveryGateInComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerDeliveryGateInRoutingModule {}
