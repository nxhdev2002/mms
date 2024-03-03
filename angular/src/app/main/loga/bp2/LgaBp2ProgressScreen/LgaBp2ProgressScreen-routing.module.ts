import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LgaBp2ProgressScreenComponent } from './LgaBp2ProgressScreen.component';

const routes: Routes = [{
    path: '',
    component: LgaBp2ProgressScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class LgaBp2ProgressScreenRoutingModule {}
