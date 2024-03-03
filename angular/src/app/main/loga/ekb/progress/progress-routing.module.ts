import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ProgressComponent } from './progress.component';

const routes: Routes = [{
    path: '',
    component: ProgressComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProgressRoutingModule {}
