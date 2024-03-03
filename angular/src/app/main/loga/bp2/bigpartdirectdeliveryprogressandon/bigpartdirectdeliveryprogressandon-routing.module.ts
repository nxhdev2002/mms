import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BigPartDirectDeliveryProgressAndonComponent } from './bigpartdirectdeliveryprogressandon.component';

const routes: Routes = [{
    path: '',
    component: BigPartDirectDeliveryProgressAndonComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BigPartDirectDeliveryProgressAndonRoutingModule {}
