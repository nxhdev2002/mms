import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { ModuleCallingComponent } from './modulecalling.component';

const routes: Routes = [{
    path: '',
    component: ModuleCallingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ModuleCallingRoutingModule {}
