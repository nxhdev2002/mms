import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { DemDetDaysComponent } from './demdetdays.component';

const routes: Routes = [{
    path: '',
    component: DemDetDaysComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DemDetDaysRoutingModule {}
