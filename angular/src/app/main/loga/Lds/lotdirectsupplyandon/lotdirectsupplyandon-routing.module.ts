import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LotDirectSupplyAndonComponent } from './lotdirectsupplyandon.component';


const routes: Routes = [{
    path: '',
    component: LotDirectSupplyAndonComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LotDirectSupplyAndonRoutingModule {}
