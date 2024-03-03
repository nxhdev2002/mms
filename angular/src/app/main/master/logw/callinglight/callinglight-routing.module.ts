import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CallingLightComponent } from './callinglight.component';

const routes: Routes = [{
    path: '',
    component: CallingLightComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CallingLightRoutingModule {}
