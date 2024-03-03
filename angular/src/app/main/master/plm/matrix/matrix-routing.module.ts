import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { MatrixComponent } from './matrix.component';

const routes: Routes = [{
    path: '',
    component: MatrixComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MatrixRoutingModule {}
