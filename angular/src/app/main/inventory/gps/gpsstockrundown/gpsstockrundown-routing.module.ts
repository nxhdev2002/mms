import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsStockRundownComponent } from './gpsstockrundown.component';

const routes: Routes = [{
    path: '',
    component: GpsStockRundownComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsStockRundownRoutingModule {}
