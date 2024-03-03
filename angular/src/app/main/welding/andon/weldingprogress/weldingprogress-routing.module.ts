import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { WeldingProgressComponent } from './weldingprogress.component';

const routes: Routes = [{
    path: '',
    component: WeldingProgressComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WeldingProgressRoutingModule {}
