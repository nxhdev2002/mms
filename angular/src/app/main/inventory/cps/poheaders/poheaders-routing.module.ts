import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PoHeadersComponent } from './poheaders.component';

const routes: Routes = [{
    path: '',
    component: PoHeadersComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PoHeadersRoutingModule {}
