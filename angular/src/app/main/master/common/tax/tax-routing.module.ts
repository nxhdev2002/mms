import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { TaxComponent } from './tax.component';

const routes: Routes = [{
    path: '',
    component: TaxComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaxRoutingModule {}
