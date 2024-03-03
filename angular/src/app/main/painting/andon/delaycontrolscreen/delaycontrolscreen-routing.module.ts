import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { DelaycontrolscreenComponent } from './delaycontrolscreen.component';


const routes: Routes = [{
    path: '',
    component: DelaycontrolscreenComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WdPunchIndicatorRoutingModule {}
