import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { MMCheckingRuleComponent } from './mmcheckingrule.component';

const routes: Routes = [{
    path: '',
    component: MMCheckingRuleComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MMCheckingRuleRoutingModule {}
