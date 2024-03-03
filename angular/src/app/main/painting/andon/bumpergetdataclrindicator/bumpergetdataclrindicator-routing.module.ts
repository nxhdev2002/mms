import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { BumpergetdataclrindicatorComponent } from './bumpergetdataclrindicator.component';


const routes: Routes = [{
    path: '',
    component: BumpergetdataclrindicatorComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BumperGetDataClrinDicatorRoutingModule {}
