import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { DevanningCaseTypeComponent } from './devanningcasetype.component';

const routes: Routes = [{
    path: '',
    component: DevanningCaseTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DevanningCaseTypeRoutingModule {}
