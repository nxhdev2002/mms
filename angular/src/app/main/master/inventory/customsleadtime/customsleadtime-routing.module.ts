import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CustomsLeadtimeComponent } from './customsleadtime.component';

const routes: Routes = [{
    path: '',
    component: CustomsLeadtimeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomsLeadtimeRoutingModule {}
