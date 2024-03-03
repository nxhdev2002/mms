import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { EciPartComponent } from './ecipart.component';

const routes: Routes = [{
    path: '',
    component: EciPartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EciPartRoutingModule {}
