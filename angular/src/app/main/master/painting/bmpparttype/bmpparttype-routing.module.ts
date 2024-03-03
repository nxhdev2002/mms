import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BmpPartTypeComponent } from './bmpparttype.component';

const routes: Routes = [{
    path: '',
    component: BmpPartTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BmpPartTypeRoutingModule {}
