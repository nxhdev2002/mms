import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { SmqdOrderLeadTimeComponent } from './smqdorderleadtime.component';

const routes: Routes = [{
    path: '',
    component: SmqdOrderLeadTimeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SmqdOrderLeadTimeRoutingModule {}
