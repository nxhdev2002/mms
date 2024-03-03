import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { DriveTrainComponent } from './drivetrain.component';

const routes: Routes = [{
    path: '',
    component: DriveTrainComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DriveTrainRoutingModule {}
export { DriveTrainComponent };

