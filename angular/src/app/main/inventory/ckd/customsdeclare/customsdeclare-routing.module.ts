import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CustomsDeclareModalComponent } from './customsdeclare.component';

const routes: Routes = [{
    path: '',
    component: CustomsDeclareModalComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomsDeclareRoutingModule {}
