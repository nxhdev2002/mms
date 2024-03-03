import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BusinessParterComponent } from './businessparter.component';

const routes: Routes = [{
    path: '',
    component: BusinessParterComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessParterRoutingModule {}
