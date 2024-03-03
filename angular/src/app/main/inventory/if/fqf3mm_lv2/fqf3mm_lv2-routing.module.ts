import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { FQF3MM_LV2Component } from './fqf3mm_lv2.component';

const routes: Routes = [{
    path: '',
    component: FQF3MM_LV2Component,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FQF3MM_LV2RoutingModule {}
