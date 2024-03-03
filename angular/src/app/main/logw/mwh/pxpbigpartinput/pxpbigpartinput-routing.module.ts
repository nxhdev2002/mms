import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PxpbigpartinputComponent } from './pxpbigpartinput.component';


const routes: Routes = [{
    path: '',
    component: PxpbigpartinputComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ],
    exports: [RouterModule],
})

export class PxpbigpartinputRoutingModule {}
