import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsCalendarComponent } from './gpscalendar.component';

const routes: Routes = [{
    path: '',
    component: GpsCalendarComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsCalendarRoutingModule {}