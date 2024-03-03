import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PickingTabletAndonComponent } from './pickingtabletandon.component';

const routes: Routes = [{
    path: '',
    component: PickingTabletAndonComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class PickingTabletAndonRoutingModule {}
