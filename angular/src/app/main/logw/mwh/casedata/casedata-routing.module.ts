import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CaseDataComponent } from './casedata.component';

const routes: Routes = [{
    path: '',
    component: CaseDataComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseDataRoutingModule {}
