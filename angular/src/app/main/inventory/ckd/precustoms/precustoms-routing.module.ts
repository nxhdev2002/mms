import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PreCustomsComponent } from './precustoms.component';

const routes: Routes = [{
    path: '',
    component: PreCustomsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PreCustomsRoutingModule {}
