import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CustomsStatusComponent } from './customsstatus.component';

const routes: Routes = [{
    path: '',
    component: CustomsStatusComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomsStatusRoutingModule {}
