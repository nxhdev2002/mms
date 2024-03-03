import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BumperComponent } from './bumper.component';

const routes: Routes = [{
    path: '',
    component: BumperComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BumperRoutingModule {}
