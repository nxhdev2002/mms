import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PlantComponent } from './plant.component';

const routes: Routes = [{
    path: '',
    component: PlantComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PlantRoutingModule {}
