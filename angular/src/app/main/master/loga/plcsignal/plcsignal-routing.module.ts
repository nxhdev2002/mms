import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PlcSignalComponent } from './plcsignal.component';

const routes: Routes = [{
    path: '',
    component: PlcSignalComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PlcSignalRoutingModule {}
