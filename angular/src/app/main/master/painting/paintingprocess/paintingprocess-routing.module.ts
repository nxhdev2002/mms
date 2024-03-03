import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PaintingProcessComponent } from './paintingprocess.component';

const routes: Routes = [{
    path: '',
    component: PaintingProcessComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaintingProcessRoutingModule {}
