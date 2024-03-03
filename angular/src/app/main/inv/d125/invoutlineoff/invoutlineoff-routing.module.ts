import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InvOutLineOffComponent } from './invoutlineoff.component';

const routes: Routes = [{
    path: '',
    component: InvOutLineOffComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvOutLineOffRoutingModule {}
