import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BumperInComponent } from './bumperin.component';


const routes: Routes = [{
    path: '',
    component: BumperInComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BumperInRoutingModule {}
