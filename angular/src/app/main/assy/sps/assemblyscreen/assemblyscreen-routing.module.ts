import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AssemblyScreenComponent } from './assemblyscreen.component';

const routes: Routes = [{
    path: '',
    component: AssemblyScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class AssemblyScreenRoutingModule {}
