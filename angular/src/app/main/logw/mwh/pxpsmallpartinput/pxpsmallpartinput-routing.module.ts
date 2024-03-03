import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PxpsmallpartinputComponent } from './pxpsmallpartinput.component';

const routes: Routes = [{
    path: '',
    component: PxpsmallpartinputComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class PxpsmallpartinputRoutingModule {}
