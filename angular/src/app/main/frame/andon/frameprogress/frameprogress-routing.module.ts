import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FrameProgressComponent } from './frameprogress.component';

const routes: Routes = [{
    path: '',
    component: FrameProgressComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FrameProgressRoutingModule { }
