import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PaintingProgressComponent } from './paintingprogress.component';

const routes: Routes = [{
    path: '',
    component: PaintingProgressComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaintingProgressRoutingModule {}
