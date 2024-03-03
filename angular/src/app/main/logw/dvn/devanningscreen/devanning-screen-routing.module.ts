import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DevanningScreenComponent } from './devanning-screen.component';

const routes: Routes = [{
    path: '',
    component: DevanningScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DevanningScreenRoutingModule { }
