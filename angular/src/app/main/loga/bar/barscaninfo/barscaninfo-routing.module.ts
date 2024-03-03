import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BarScanInfoComponent } from './barscaninfo.component';

const routes: Routes = [{
    path: '',
    component: BarScanInfoComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BarScanInfoRoutingModule {}
