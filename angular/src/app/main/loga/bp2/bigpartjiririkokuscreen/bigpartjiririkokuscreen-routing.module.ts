import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BigPartJiririkokuScreenComponent } from './bigpartjiririkokuscreen.component';

const routes: Routes = [{
    path: '',
    component: BigPartJiririkokuScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BigPartJiririkokuScreenRoutingModule {}
