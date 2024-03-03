import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { DailyWorkingTimeComponent } from './dailyworkingtime.component';

const routes: Routes = [{
    path: '',
    component: DailyWorkingTimeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DailyWorkingTimeRoutingModule {}
