import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LgaBp2ProgressMonitorScreenComponent } from './LgaBp2ProgressMonitorScreen.component';

const routes: Routes = [{
    path: '',
    component: LgaBp2ProgressMonitorScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class LgaBp2ProgressMonitorScreenRoutingModule {}
