import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PIOPartTypeComponent } from './pioparttype.component';

const routes: Routes = [{
    path: '',
    component: PIOPartTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PIOPartTypeRoutingModule {}
