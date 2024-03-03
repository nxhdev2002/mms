import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LotPartComponent } from './lotpart.component';

const routes: Routes = [{
    path: '',
    component: LotPartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LotPartRoutingModule {}
