import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsScreenSettingComponent } from './gpsscreensetting.component';

const routes: Routes = [{
    path: '',
    component: GpsScreenSettingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsScreenSettingRoutingModule {}
