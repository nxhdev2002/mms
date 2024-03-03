import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PartComponent } from './part.component';

const routes: Routes = [{
    path: '',
    component: PartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PartRoutingModule {}
