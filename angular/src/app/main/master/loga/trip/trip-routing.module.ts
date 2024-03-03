import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { TripComponent } from './trip.component';

const routes: Routes = [{
    path: '',
    component: TripComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TripRoutingModule {}
