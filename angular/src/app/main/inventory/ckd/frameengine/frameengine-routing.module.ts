import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { FrameEngineComponent } from './frameengine.component';

const routes: Routes = [{
    path: '',
    component: FrameEngineComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FrameEngineRoutingModule {}
