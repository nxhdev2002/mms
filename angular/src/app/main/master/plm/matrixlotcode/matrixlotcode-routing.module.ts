import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { MatrixLotCodeComponent } from './matrixlotcode.component';

const routes: Routes = [{
    path: '',
    component: MatrixLotCodeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MatrixLotCodeRoutingModule {}
