import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CpsSuppliersComponent } from './cpssuppliers.component';

const routes: Routes = [{
    path: '',
    component: CpsSuppliersComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CpsSuppliersRoutingModule {}
