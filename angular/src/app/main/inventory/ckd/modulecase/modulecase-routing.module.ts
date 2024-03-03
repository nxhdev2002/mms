
import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ModuleCaseComponent } from './modulecase.component';

const routes: Routes = [{
    path: '',
    component: ModuleCaseComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ModuleCaseRoutingModule {}
