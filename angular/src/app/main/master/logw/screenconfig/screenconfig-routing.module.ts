import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ScreenConfigComponent } from './screenconfig.component';

const routes: Routes = [{
    path: '',
    component: ScreenConfigComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ScreenConfigRoutingModule {}
