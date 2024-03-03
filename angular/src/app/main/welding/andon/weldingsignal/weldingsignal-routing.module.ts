import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { WeldingSignalComponent } from './weldingsignal.component';

const routes: Routes = [{
    path: '',
    component: WeldingSignalComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WeldingSignalRoutingModule {}
