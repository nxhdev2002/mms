import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LotUpackingAndonComponent } from './lotupackingandon.component';

const routes: Routes = [{
    path: '',
    component: LotUpackingAndonComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LotUpackingAndonRoutingModule {}
