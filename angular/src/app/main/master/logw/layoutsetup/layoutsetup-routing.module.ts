import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LayoutSetupComponent } from './layoutsetup.component';

const routes: Routes = [{
    path: '',
    component: LayoutSetupComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LayoutSetupRoutingModule {}
