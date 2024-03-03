import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ReportRequestComponent } from './report-request.component';

const routes: Routes = [{
    path: '',
    component: ReportRequestComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ReportRequestRoutingModule {}
