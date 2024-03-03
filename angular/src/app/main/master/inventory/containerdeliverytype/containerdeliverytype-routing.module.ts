import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ContainerDeliveryTypeComponent } from './containerdeliverytype.component';

const routes: Routes = [{
    path: '',
    component: ContainerDeliveryTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerDeliveryTypeRoutingModule {}
