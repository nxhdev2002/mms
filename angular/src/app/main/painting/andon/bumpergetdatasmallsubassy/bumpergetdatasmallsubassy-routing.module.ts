import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BumpergetdatasmallsubassyComponent } from './bumpergetdatasmallsubassy.component';


const routes: Routes = [{
    path: '',
    component: BumpergetdatasmallsubassyComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BumperGetDataSmallSubAssyRoutingModule {}
