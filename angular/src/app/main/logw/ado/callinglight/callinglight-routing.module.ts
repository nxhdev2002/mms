import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CallinglightComponent } from './callinglight.component';

const routes: Routes = [{
    path: '',
    component: CallinglightComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CallinglightRoutingModule {}
