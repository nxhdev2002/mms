import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PcRackComponent } from './pcrack.component';

const routes: Routes = [{
    path: '',
    component: PcRackComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PcRackRoutingModule {}
