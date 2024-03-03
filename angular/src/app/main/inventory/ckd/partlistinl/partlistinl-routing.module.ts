import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PioPartListInlComponent } from './partlistinl.component';

const routes: Routes = [{
    path: '',
    component: PioPartListInlComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PartListInlRoutingModule {}
