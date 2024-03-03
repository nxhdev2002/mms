import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CustomsLeadTimeComponent } from './customsleadtimemaster.component';

const routes: Routes = [{
    path: '',
    component: CustomsLeadTimeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomsLeadTimeRoutingModule {}
