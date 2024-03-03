import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { JiririkokuScreenComponent } from './jiririkokuscreen.component';

const routes: Routes = [{
    path: '',
    component: JiririkokuScreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JiririkokuScreenRoutingModule {}
