import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { SapAssetMasterComponent } from './sapassetmaster.component';
const routes: Routes = [{
    path: '',
    component: SapAssetMasterComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SapAssetMasterRoutingModule {}
