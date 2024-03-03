import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LookupComponent } from './lookup.component';

const routes: Routes = [{
    path: '',
    component: LookupComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LookupRoutingModule {}
