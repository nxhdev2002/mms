import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LineRealTimeControlComponent } from './linerealtimecontrol.component';

const routes: Routes = [{
    path: '',
    component: LineRealTimeControlComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LineRealTimeControlRoutingModule {}
