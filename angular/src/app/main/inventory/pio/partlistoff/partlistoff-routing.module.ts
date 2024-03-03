import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PioPartListOffComponent } from './partlistoff.component';


const routes: Routes = [{
    path: '',
    component: PioPartListOffComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PartListOffRoutingModule {}
