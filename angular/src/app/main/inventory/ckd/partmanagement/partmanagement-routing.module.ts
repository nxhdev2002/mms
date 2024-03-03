import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PartManagementComponent } from './partmanagement.component';

const routes: Routes = [{
    path: '',
    component: PartManagementComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PartManagementRoutingModule {}
