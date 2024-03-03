import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CCRMonitorComponent} from './ccrmonitor.component';

const routes: Routes = [{
    path: '',
    component: CCRMonitorComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CCRMonitorRoutingModule {}
