import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IhpPartListComponent } from './ihppartlist.component';

const routes: Routes = [{
    path: '',
    component: IhpPartListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class IhpPartListRoutingModule { }
