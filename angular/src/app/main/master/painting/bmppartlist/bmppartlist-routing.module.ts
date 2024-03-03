import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BmpPartListComponent } from './bmppartlist.component';

const routes: Routes = [{
    path: '',
    component: BmpPartListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BmpPartListRoutingModule {}
