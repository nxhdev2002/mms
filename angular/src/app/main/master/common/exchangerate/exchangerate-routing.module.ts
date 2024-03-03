import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ExchangeRateComponent } from './exchangerate.component';

const routes: Routes = [{
    path: '',
    component: ExchangeRateComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ExchangeRateRoutingModule {}
