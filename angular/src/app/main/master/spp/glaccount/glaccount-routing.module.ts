import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GLAccountComponent } from './glaccount.component';

const routes: Routes = [{
    path: '',
    component: GLAccountComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GLAccountRoutingModule {}
