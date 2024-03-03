import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PaintingDataComponent } from './paintingdata.component';

const routes: Routes = [{
    path: '',
    component: PaintingDataComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaintingDataRoutingModule {}
