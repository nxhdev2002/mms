import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProcessInstructionComponent } from './processinstruction.component';

const routes: Routes = [{
    path: '',
    component: ProcessInstructionComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProcessInstructionRoutingModule {}
