import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LogAPlcSignalComponent } from './signal.component';

const routes: Routes = [{
    path: '',
    component: LogAPlcSignalComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SignalRoutingModule {}
