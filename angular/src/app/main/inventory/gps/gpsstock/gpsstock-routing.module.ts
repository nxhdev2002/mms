import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsStockComponent } from './gpsstock.component';

const routes: Routes = [{
    path: '',
    component: GpsStockComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsStockRoutingModule {}
