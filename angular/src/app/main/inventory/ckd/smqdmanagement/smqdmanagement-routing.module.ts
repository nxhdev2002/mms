import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SmqdComponent } from './smqdmanagement.component';

const routes: Routes = [{
    path: '',
    component: SmqdComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SmqdRoutingModule { }
