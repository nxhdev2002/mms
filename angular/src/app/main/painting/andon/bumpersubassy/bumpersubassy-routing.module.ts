import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BumperSubAssyComponent } from './bumpersubassy.component';


const routes: Routes = [{
    path: '',
    component: BumperSubAssyComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BumperSubAssyRoutingModule {}
